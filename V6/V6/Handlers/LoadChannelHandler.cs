using System;
using System.Threading.Tasks;

namespace GJVdc32Tool.Handlers
{
    /// <summary>
    /// 负载通道控制处理器
    /// 职责：处理 GJDD-750 通道的开启/关闭/配置操作
    /// </summary>
    public class LoadChannelHandler
    {
        #region 常量定义

        /// <summary>
        /// 通道数量
        /// </summary>
        public const int CHANNEL_COUNT = 8;

        /// <summary>
        /// 最小电流值 (A)
        /// </summary>
        public const double MIN_CURRENT = 0.0;

        /// <summary>
        /// 最大电流值 (A)
        /// </summary>
        public const double MAX_CURRENT = 60.0;

        /// <summary>
        /// 操作超时 (ms)
        /// </summary>
        private const int OPERATION_TIMEOUT_MS = 3000;

        /// <summary>
        /// 操作间隔延迟 (ms)
        /// </summary>
        private const int OPERATION_DELAY_MS = 50;

        #endregion

        #region 私有字段

        private readonly Func<int, bool, Task<bool>> _setChannelState;
        private readonly Func<int, double, Task<bool>> _setChannelCurrent;
        private readonly Func<double, Task<bool>> _setAllChannelsCurrent;
        private readonly Func<bool, Task<bool>> _setAllChannelsState;
        private readonly Action<string, bool?> _logAction;

        private readonly bool[] _channelStates;
        private readonly double[] _channelCurrents;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建负载通道处理器
        /// </summary>
        public LoadChannelHandler(
            Func<int, bool, Task<bool>> setChannelState,
            Func<int, double, Task<bool>> setChannelCurrent,
            Func<double, Task<bool>> setAllChannelsCurrent,
            Func<bool, Task<bool>> setAllChannelsState,
            Action<string, bool?> logAction = null)
        {
            _setChannelState = setChannelState ?? throw new ArgumentNullException(nameof(setChannelState));
            _setChannelCurrent = setChannelCurrent ?? throw new ArgumentNullException(nameof(setChannelCurrent));
            _setAllChannelsCurrent = setAllChannelsCurrent ?? throw new ArgumentNullException(nameof(setAllChannelsCurrent));
            _setAllChannelsState = setAllChannelsState ?? throw new ArgumentNullException(nameof(setAllChannelsState));
            _logAction = logAction ?? ((msg, success) => { });

            _channelStates = new bool[CHANNEL_COUNT];
            _channelCurrents = new double[CHANNEL_COUNT];
        }

        #endregion

        #region 公共方法 - 单通道操作

        /// <summary>
        /// 切换通道状态（开/关）
        /// </summary>
        /// <param name="channelIndex">通道索引 (0-7)</param>
        /// <returns>操作结果</returns>
        public async Task<ChannelOperationResult> ToggleChannelAsync(int channelIndex)
        {
            if (!ValidateChannelIndex(channelIndex))
            {
                return ChannelOperationResult.Fail($"无效的通道索引: {channelIndex}");
            }

            bool newState = !_channelStates[channelIndex];
            return await SetChannelStateAsync(channelIndex, newState);
        }

        /// <summary>
        /// 设置通道状态
        /// </summary>
        public async Task<ChannelOperationResult> SetChannelStateAsync(int channelIndex, bool isOn)
        {
            if (!ValidateChannelIndex(channelIndex))
            {
                return ChannelOperationResult.Fail($"无效的通道索引: {channelIndex}");
            }

            string action = isOn ? "开启" : "关闭";
            _logAction($"正在{action}通道 {channelIndex + 1}...", null);

            try
            {
                bool success = await _setChannelState(channelIndex, isOn);

                if (success)
                {
                    _channelStates[channelIndex] = isOn;
                    _logAction($"通道 {channelIndex + 1} 已{action}", true);
                    return ChannelOperationResult.Ok($"通道 {channelIndex + 1} 已{action}");
                }
                else
                {
                    _logAction($"通道 {channelIndex + 1} {action}失败", false);
                    return ChannelOperationResult.Fail($"{action}失败");
                }
            }
            catch (Exception ex)
            {
                _logAction($"通道操作异常: {ex.Message}", false);
                return ChannelOperationResult.Fail($"操作异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置通道电流
        /// </summary>
        public async Task<ChannelOperationResult> SetChannelCurrentAsync(int channelIndex, double current)
        {
            if (!ValidateChannelIndex(channelIndex))
            {
                return ChannelOperationResult.Fail($"无效的通道索引: {channelIndex}");
            }

            if (!ValidateCurrent(current))
            {
                return ChannelOperationResult.Fail($"电流值超出范围 ({MIN_CURRENT}-{MAX_CURRENT}A)");
            }

            _logAction($"正在设置通道 {channelIndex + 1} 电流为 {current:F2}A...", null);

            try
            {
                bool success = await _setChannelCurrent(channelIndex, current);

                if (success)
                {
                    _channelCurrents[channelIndex] = current;
                    _logAction($"通道 {channelIndex + 1} 电流已设置为 {current:F2}A", true);
                    return ChannelOperationResult.Ok($"电流设置成功");
                }
                else
                {
                    _logAction($"通道 {channelIndex + 1} 电流设置失败", false);
                    return ChannelOperationResult.Fail("电流设置失败");
                }
            }
            catch (Exception ex)
            {
                _logAction($"设置电流异常: {ex.Message}", false);
                return ChannelOperationResult.Fail($"操作异常: {ex.Message}");
            }
        }

        #endregion

        #region 公共方法 - 批量操作

        /// <summary>
        /// 开启所有通道
        /// </summary>
        public async Task<ChannelOperationResult> TurnOnAllChannelsAsync()
        {
            return await SetAllChannelsStateAsync(true);
        }

        /// <summary>
        /// 关闭所有通道
        /// </summary>
        public async Task<ChannelOperationResult> TurnOffAllChannelsAsync()
        {
            return await SetAllChannelsStateAsync(false);
        }

        /// <summary>
        /// 设置所有通道状态
        /// </summary>
        public async Task<ChannelOperationResult> SetAllChannelsStateAsync(bool isOn)
        {
            string action = isOn ? "开启" : "关闭";
            _logAction($"正在{action}所有通道...", null);

            try
            {
                bool success = await _setAllChannelsState(isOn);

                if (success)
                {
                    for (int i = 0; i < CHANNEL_COUNT; i++)
                    {
                        _channelStates[i] = isOn;
                    }
                    _logAction($"所有通道已{action}", true);
                    return ChannelOperationResult.Ok($"所有通道已{action}");
                }
                else
                {
                    _logAction($"批量{action}失败", false);
                    return ChannelOperationResult.Fail($"批量{action}失败");
                }
            }
            catch (Exception ex)
            {
                _logAction($"批量操作异常: {ex.Message}", false);
                return ChannelOperationResult.Fail($"操作异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置所有通道电流
        /// </summary>
        public async Task<ChannelOperationResult> SetAllChannelsCurrentAsync(double current)
        {
            if (!ValidateCurrent(current))
            {
                return ChannelOperationResult.Fail($"电流值超出范围 ({MIN_CURRENT}-{MAX_CURRENT}A)");
            }

            _logAction($"正在设置所有通道电流为 {current:F2}A...", null);

            try
            {
                bool success = await _setAllChannelsCurrent(current);

                if (success)
                {
                    for (int i = 0; i < CHANNEL_COUNT; i++)
                    {
                        _channelCurrents[i] = current;
                    }
                    _logAction($"所有通道电流已设置为 {current:F2}A", true);
                    return ChannelOperationResult.Ok("批量电流设置成功");
                }
                else
                {
                    _logAction("批量电流设置失败", false);
                    return ChannelOperationResult.Fail("批量电流设置失败");
                }
            }
            catch (Exception ex)
            {
                _logAction($"批量设置电流异常: {ex.Message}", false);
                return ChannelOperationResult.Fail($"操作异常: {ex.Message}");
            }
        }

        #endregion

        #region 公共方法 - 状态查询

        /// <summary>
        /// 获取通道状态
        /// </summary>
        public bool GetChannelState(int channelIndex)
        {
            if (!ValidateChannelIndex(channelIndex))
                return false;

            return _channelStates[channelIndex];
        }

        /// <summary>
        /// 获取通道电流设定值
        /// </summary>
        public double GetChannelCurrent(int channelIndex)
        {
            if (!ValidateChannelIndex(channelIndex))
                return 0;

            return _channelCurrents[channelIndex];
        }

        /// <summary>
        /// 更新通道状态缓存（从轮询数据更新）
        /// </summary>
        public void UpdateChannelStates(LoadChannelData[] channels)
        {
            if (channels == null) return;

            for (int i = 0; i < Math.Min(channels.Length, CHANNEL_COUNT); i++)
            {
                _channelStates[i] = channels[i].IsOn;
                _channelCurrents[i] = channels[i].Current;
            }
        }

        #endregion

        #region 私有方法

        private bool ValidateChannelIndex(int channelIndex)
        {
            return channelIndex >= 0 && channelIndex < CHANNEL_COUNT;
        }

        private bool ValidateCurrent(double current)
        {
            return current >= MIN_CURRENT && current <= MAX_CURRENT;
        }

        #endregion
    }

    /// <summary>
    /// 通道操作结果
    /// </summary>
    public class ChannelOperationResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        private ChannelOperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static ChannelOperationResult Ok(string message) => new ChannelOperationResult(true, message);
        public static ChannelOperationResult Fail(string message) => new ChannelOperationResult(false, message);
    }
}
