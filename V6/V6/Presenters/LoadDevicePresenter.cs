using System;
using System.Threading;
using System.Threading.Tasks;
using GJVdc32Tool.Handlers;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Presenters
{
    /// <summary>
    /// GJDD-750 负载设备 Presenter
    /// 职责：管理 GJDD-750 负载设备的所有业务逻辑
    /// </summary>
    public class LoadDevicePresenter : IDisposable
    {
        #region 私有字段

        private readonly ILoadDeviceView _view;
        private readonly IPollingCoordinator _pollingCoordinator;
        private readonly DataReadHandler _dataReadHandler;
        private readonly LoadChannelHandler _channelHandler;
        private readonly ChannelDisplayHandler _displayHandler;
        private readonly Action<string, bool?> _logAction;

        private bool _disposed = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建负载设备 Presenter
        /// </summary>
        public LoadDevicePresenter(LoadDevicePresenterConfig config)
        {
            ValidateConfig(config);

            _view = config.View;
            _pollingCoordinator = config.PollingCoordinator;
            _dataReadHandler = config.DataReadHandler;
            _channelHandler = config.ChannelHandler;
            _displayHandler = config.DisplayHandler;
            _logAction = config.LogAction ?? ((msg, success) => { });

            BindEvents();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            ResetDisplay();
        }

        /// <summary>
        /// 轮询开始时调用
        /// </summary>
        public void OnPollingStarted()
        {
            _logAction("GJDD-750 数据轮询已启动", true);
        }

        /// <summary>
        /// 重置显示
        /// </summary>
        public void ResetDisplay()
        {
            _displayHandler?.ResetLoadChannels();

            if (_view != null)
            {
                _view.InverterVoltage = "--.- V";
                _view.InverterCurrent = "--.- A";
                _view.InverterPower = "--.- W";
                _view.InverterStatus = "离线";
            }
        }

        /// <summary>
        /// 执行单次数据读取
        /// </summary>
        public async Task<bool> ReadDataOnceAsync(CancellationToken token)
        {
            var result = await _dataReadHandler.ReadLoadChannelsAsync(token);

            if (result.Success)
            {
                _displayHandler?.UpdateLoadChannels(result.Channels);
                _channelHandler?.UpdateChannelStates(result.Channels);
                return true;
            }

            if (!result.IsCancelled)
            {
                _logAction($"读取数据失败: {result.ErrorMessage}", false);
            }

            return false;
        }

        /// <summary>
        /// 处理通道开关切换
        /// </summary>
        public async Task HandleChannelToggleAsync(int channelIndex)
        {
            await _pollingCoordinator.PauseAndExecuteAsync(async () =>
            {
                var result = await _channelHandler.ToggleChannelAsync(channelIndex);

                if (!result.Success)
                {
                    _view?.ShowChannelError(channelIndex, result.Message);
                }
            });
        }

        /// <summary>
        /// 处理设置通道电流
        /// </summary>
        public async Task HandleSetChannelCurrentAsync(int channelIndex, double current)
        {
            await _pollingCoordinator.PauseAndExecuteAsync(async () =>
            {
                var result = await _channelHandler.SetChannelCurrentAsync(channelIndex, current);

                if (!result.Success)
                {
                    _view?.ShowChannelError(channelIndex, result.Message);
                }
            });
        }

        /// <summary>
        /// 处理批量开启
        /// </summary>
        public async Task HandleTurnOnAllAsync()
        {
            await _pollingCoordinator.PauseAndExecuteAsync(async () =>
            {
                var result = await _channelHandler.TurnOnAllChannelsAsync();

                if (!result.Success)
                {
                    _logAction(result.Message, false);
                }
            });
        }

        /// <summary>
        /// 处理批量关闭
        /// </summary>
        public async Task HandleTurnOffAllAsync()
        {
            await _pollingCoordinator.PauseAndExecuteAsync(async () =>
            {
                var result = await _channelHandler.TurnOffAllChannelsAsync();

                if (!result.Success)
                {
                    _logAction(result.Message, false);
                }
            });
        }

        /// <summary>
        /// 处理批量设置电流
        /// </summary>
        public async Task HandleSetAllCurrentAsync(double current)
        {
            await _pollingCoordinator.PauseAndExecuteAsync(async () =>
            {
                var result = await _channelHandler.SetAllChannelsCurrentAsync(current);

                if (!result.Success)
                {
                    _logAction(result.Message, false);
                }
            });
        }

        /// <summary>
        /// 更新变频器状态显示
        /// </summary>
        public void UpdateInverterStatus(double voltage, double current, double power, bool isOnline)
        {
            if (_view == null) return;

            _view.InverterVoltage = $"{voltage:F1} V";
            _view.InverterCurrent = $"{current:F2} A";
            _view.InverterPower = $"{power:F1} W";
            _view.InverterStatus = isOnline ? "在线" : "离线";
        }

        #endregion

        #region 私有方法

        private void ValidateConfig(LoadDevicePresenterConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (config.View == null)
                throw new ArgumentNullException(nameof(config.View));
            if (config.PollingCoordinator == null)
                throw new ArgumentNullException(nameof(config.PollingCoordinator));
            if (config.DataReadHandler == null)
                throw new ArgumentNullException(nameof(config.DataReadHandler));
            if (config.ChannelHandler == null)
                throw new ArgumentNullException(nameof(config.ChannelHandler));
            if (config.DisplayHandler == null)
                throw new ArgumentNullException(nameof(config.DisplayHandler));
        }

        private void BindEvents()
        {
            _pollingCoordinator.PollingCycleCompleted += OnPollingCycleCompleted;

            if (_view != null)
            {
                _view.ChannelToggleRequested += OnChannelToggleRequested;
                _view.CurrentSetRequested += OnCurrentSetRequested;
                _view.BatchTurnOnRequested += OnBatchTurnOnRequested;
                _view.BatchTurnOffRequested += OnBatchTurnOffRequested;
                _view.BatchCurrentSetRequested += OnBatchCurrentSetRequested;
            }
        }

        private void UnbindEvents()
        {
            _pollingCoordinator.PollingCycleCompleted -= OnPollingCycleCompleted;

            if (_view != null)
            {
                _view.ChannelToggleRequested -= OnChannelToggleRequested;
                _view.CurrentSetRequested -= OnCurrentSetRequested;
                _view.BatchTurnOnRequested -= OnBatchTurnOnRequested;
                _view.BatchTurnOffRequested -= OnBatchTurnOffRequested;
                _view.BatchCurrentSetRequested -= OnBatchCurrentSetRequested;
            }
        }

        private void OnPollingCycleCompleted(object sender, PollingDataEventArgs e)
        {
            // 轮询完成后的处理
        }

        private async void OnChannelToggleRequested(object sender, int channelIndex)
        {
            await HandleChannelToggleAsync(channelIndex);
        }

        private async void OnCurrentSetRequested(object sender, ChannelCurrentEventArgs e)
        {
            await HandleSetChannelCurrentAsync(e.ChannelIndex, e.Current);
        }

        private async void OnBatchTurnOnRequested(object sender, EventArgs e)
        {
            await HandleTurnOnAllAsync();
        }

        private async void OnBatchTurnOffRequested(object sender, EventArgs e)
        {
            await HandleTurnOffAllAsync();
        }

        private async void OnBatchCurrentSetRequested(object sender, double current)
        {
            await HandleSetAllCurrentAsync(current);
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
                UnbindEvents();
            }

            _disposed = true;
        }

        #endregion
    }

    /// <summary>
    /// LoadDevice Presenter 配置
    /// </summary>
    public class LoadDevicePresenterConfig
    {
        public ILoadDeviceView View { get; set; }
        public IPollingCoordinator PollingCoordinator { get; set; }
        public DataReadHandler DataReadHandler { get; set; }
        public LoadChannelHandler ChannelHandler { get; set; }
        public ChannelDisplayHandler DisplayHandler { get; set; }
        public Action<string, bool?> LogAction { get; set; }
    }
}
