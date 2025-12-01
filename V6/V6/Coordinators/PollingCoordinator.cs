using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Coordinators
{
    /// <summary>
    /// 轮询协调器
    /// 职责：管理设备数据轮询的生命周期，防止轮询任务漂移和资源泄漏
    /// </summary>
    /// <remarks>
    /// 架构规则：
    /// - 使用 CancellationToken 进行安全取消
    /// - 异常不会被吞掉，通过事件上报
    /// - 轮询间隔使用常量，避免魔法数字
    /// </remarks>
    public class PollingCoordinator : IPollingCoordinator, IDisposable
    {
        #region 常量定义

        /// <summary>
        /// 默认轮询间隔 (毫秒)
        /// </summary>
        public const int DEFAULT_POLLING_INTERVAL_MS = 500;

        /// <summary>
        /// 轮询停止超时时间 (毫秒)
        /// </summary>
        public const int POLLING_STOP_TIMEOUT_MS = 1000;

        /// <summary>
        /// UI 更新延迟 (毫秒)
        /// </summary>
        public const int UI_UPDATE_DELAY_MS = 100;

        /// <summary>
        /// 最大连续错误次数
        /// </summary>
        public const int MAX_CONSECUTIVE_ERRORS = 5;

        #endregion

        #region 私有字段

        private readonly object _lockObject = new object();
        private CancellationTokenSource _pollingCts;
        private PollingState _state = PollingState.Stopped;
        private int _pollingInterval = DEFAULT_POLLING_INTERVAL_MS;
        private long _completedCycles = 0;
        private int _consecutiveErrors = 0;
        private bool _isPaused = false;
        private bool _disposed = false;

        // 轮询委托
        private readonly Func<CancellationToken, Task<bool>> _vdc32PollingAction;
        private readonly Func<CancellationToken, Task<bool>> _loadDevicePollingAction;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建轮询协调器
        /// </summary>
        /// <param name="vdc32PollingAction">VDC-32 轮询操作委托，返回是否有有效数据</param>
        /// <param name="loadDevicePollingAction">负载设备轮询操作委托，返回是否有有效数据</param>
        public PollingCoordinator(
            Func<CancellationToken, Task<bool>> vdc32PollingAction,
            Func<CancellationToken, Task<bool>> loadDevicePollingAction)
        {
            _vdc32PollingAction = vdc32PollingAction ?? throw new ArgumentNullException(nameof(vdc32PollingAction));
            _loadDevicePollingAction = loadDevicePollingAction ?? throw new ArgumentNullException(nameof(loadDevicePollingAction));
        }

        #endregion

        #region IPollingCoordinator 实现

        /// <inheritdoc/>
        public PollingState State
        {
            get
            {
                lock (_lockObject)
                {
                    return _state;
                }
            }
            private set
            {
                lock (_lockObject)
                {
                    if (_state != value)
                    {
                        _state = value;
                        OnStateChanged(value);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public bool IsPolling => State == PollingState.Running || State == PollingState.Paused;

        /// <inheritdoc/>
        public int PollingInterval
        {
            get => _pollingInterval;
            set
            {
                if (value < 100 || value > 10000)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "轮询间隔范围: 100-10000ms");
                }
                _pollingInterval = value;
            }
        }

        /// <inheritdoc/>
        public long CompletedCycles => Interlocked.Read(ref _completedCycles);

        /// <inheritdoc/>
        public int ConsecutiveErrors => _consecutiveErrors;

        /// <inheritdoc/>
        public async Task StartVdc32PollingAsync()
        {
            await StartPollingAsync(_vdc32PollingAction);
        }

        /// <inheritdoc/>
        public async Task StartLoadDevicePollingAsync()
        {
            await StartPollingAsync(_loadDevicePollingAction);
        }

        /// <inheritdoc/>
        public async Task StopAsync(bool waitForComplete = true)
        {
            if (State == PollingState.Stopped)
            {
                return;
            }

            State = PollingState.Stopping;

            // 请求取消
            _pollingCts?.Cancel();

            if (waitForComplete)
            {
                // 等待轮询任务完成
                int waitCount = 0;
                while (State == PollingState.Stopping && waitCount < (POLLING_STOP_TIMEOUT_MS / UI_UPDATE_DELAY_MS))
                {
                    await Task.Delay(UI_UPDATE_DELAY_MS);
                    waitCount++;
                }
            }

            // 清理资源
            DisposeCancellationToken();
            State = PollingState.Stopped;
        }

        /// <inheritdoc/>
        public void Pause()
        {
            if (State == PollingState.Running)
            {
                _isPaused = true;
                State = PollingState.Paused;
            }
        }

        /// <inheritdoc/>
        public void Resume()
        {
            if (State == PollingState.Paused)
            {
                _isPaused = false;
                State = PollingState.Running;
            }
        }

        /// <inheritdoc/>
        public async Task PauseAndExecuteAsync(Func<Task> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            bool wasRunning = State == PollingState.Running;
            
            if (wasRunning)
            {
                Pause();
                
                // 等待当前轮询周期完成
                await Task.Delay(UI_UPDATE_DELAY_MS * 2);
            }

            try
            {
                await action();
            }
            finally
            {
                if (wasRunning)
                {
                    Resume();
                }
            }
        }

        #endregion

        #region 事件

        /// <inheritdoc/>
        public event EventHandler<PollingDataEventArgs> PollingCycleCompleted;

        /// <inheritdoc/>
        public event EventHandler<Exception> PollingError;

        /// <inheritdoc/>
        public event EventHandler<PollingState> StateChanged;

        /// <inheritdoc/>
        public event EventHandler<int> ErrorThresholdReached;

        #endregion

        #region 私有方法

        private async Task StartPollingAsync(Func<CancellationToken, Task<bool>> pollingAction)
        {
            // 停止现有轮询
            if (IsPolling)
            {
                await StopAsync(waitForComplete: true);
            }

            // 创建新的取消令牌
            _pollingCts = new CancellationTokenSource();
            var token = _pollingCts.Token;

            // 重置计数器
            Interlocked.Exchange(ref _completedCycles, 0);
            _consecutiveErrors = 0;
            _isPaused = false;

            State = PollingState.Running;

            // 启动轮询循环（后台任务）
            _ = RunPollingLoopAsync(pollingAction, token);
        }

        private async Task RunPollingLoopAsync(Func<CancellationToken, Task<bool>> pollingAction, CancellationToken token)
        {
            var stopwatch = new Stopwatch();

            try
            {
                while (!token.IsCancellationRequested)
                {
                    // 检查暂停状态
                    if (_isPaused)
                    {
                        await Task.Delay(UI_UPDATE_DELAY_MS, token);
                        continue;
                    }

                    stopwatch.Restart();

                    try
                    {
                        bool hasValidData = await pollingAction(token);

                        stopwatch.Stop();

                        // 重置连续错误计数
                        _consecutiveErrors = 0;

                        // 更新周期计数
                        Interlocked.Increment(ref _completedCycles);

                        // 触发完成事件
                        OnPollingCycleCompleted(new PollingDataEventArgs
                        {
                            CycleNumber = CompletedCycles,
                            HasValidData = hasValidData,
                            ElapsedMs = (int)stopwatch.ElapsedMilliseconds
                        });
                    }
                    catch (OperationCanceledException)
                    {
                        // 正常取消，退出循环
                        break;
                    }
                    catch (Exception ex)
                    {
                        stopwatch.Stop();
                        _consecutiveErrors++;

                        // 判断是否为断开连接导致的预期错误
                        bool isDisconnectError = IsDisconnectException(ex);

                        if (!isDisconnectError)
                        {
                            OnPollingError(ex);
                        }

                        // 检查连续错误阈值
                        if (_consecutiveErrors >= MAX_CONSECUTIVE_ERRORS)
                        {
                            OnErrorThresholdReached(_consecutiveErrors);
                            break;
                        }

                        // 断开错误直接退出
                        if (isDisconnectError)
                        {
                            break;
                        }
                    }

                    // 等待下一个轮询周期
                    await Task.Delay(_pollingInterval, token);
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消
            }
            finally
            {
                if (State != PollingState.Stopped)
                {
                    State = PollingState.Stopped;
                }
            }
        }

        private bool IsDisconnectException(Exception ex)
        {
            string message = ex.Message ?? string.Empty;

            return message.Contains("已中止 I/O 操作") ||
                   message.Contains("串口已关闭") ||
                   message.Contains("串口未连接") ||
                   message.Contains("TCP 未连接") ||
                   message.Contains("TCP连接已关闭") ||
                   message.Contains("TCP连接已断开") ||
                   message.Contains("网络流不可用") ||
                   message.Contains("网络流已被释放") ||
                   message.Contains("由于线程退出") ||
                   ex is ObjectDisposedException ||
                   ex is System.IO.IOException ||
                   (ex is InvalidOperationException && 
                    (message.Contains("连接") || message.Contains("断开")));
        }

        private void DisposeCancellationToken()
        {
            try
            {
                _pollingCts?.Cancel();
                _pollingCts?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PollingCoordinator] 释放 CTS 异常: {ex.Message}");
            }
            finally
            {
                _pollingCts = null;
            }
        }

        private void OnPollingCycleCompleted(PollingDataEventArgs e)
        {
            PollingCycleCompleted?.Invoke(this, e);
        }

        private void OnPollingError(Exception ex)
        {
            PollingError?.Invoke(this, ex);
        }

        private void OnStateChanged(PollingState state)
        {
            StateChanged?.Invoke(this, state);
        }

        private void OnErrorThresholdReached(int errorCount)
        {
            ErrorThresholdReached?.Invoke(this, errorCount);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                DisposeCancellationToken();
            }

            _disposed = true;
        }

        #endregion
    }
}
