using System;
using System.Threading;
using System.Threading.Tasks;
using GJVdc32Tool.Handlers;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Presenters
{
    /// <summary>
    /// VDC-32 设备 Presenter
    /// 职责：管理 VDC-32 电压检测板的所有业务逻辑
    /// </summary>
    public class Vdc32Presenter : IDisposable
    {
        #region 私有字段

        private readonly IVdc32View _view;
        private readonly IPollingCoordinator _pollingCoordinator;
        private readonly DataReadHandler _dataReadHandler;
        private readonly ChannelDisplayHandler _displayHandler;
        private readonly Action<string, bool?> _logAction;

        private bool _disposed = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建 VDC-32 Presenter
        /// </summary>
        public Vdc32Presenter(Vdc32PresenterConfig config)
        {
            ValidateConfig(config);

            _view = config.View;
            _pollingCoordinator = config.PollingCoordinator;
            _dataReadHandler = config.DataReadHandler;
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
            _logAction("VDC-32 数据轮询已启动", true);
        }

        /// <summary>
        /// 重置显示
        /// </summary>
        public void ResetDisplay()
        {
            _displayHandler?.ResetVdc32Channels();

            if (_view != null)
            {
                _view.FirmwareVersion = "固件版本: --";
                _view.DeviceName = "设备名称: --";
                _view.SlaveAddress = "从机地址: --";
                _view.Temperature = "--.- ℃";
                _view.Humidity = "--.- %";
            }
        }

        /// <summary>
        /// 执行单次数据读取
        /// </summary>
        public async Task<bool> ReadDataOnceAsync(CancellationToken token)
        {
            var result = await _dataReadHandler.ReadVdc32ChannelsAsync(token);

            if (result.Success)
            {
                _displayHandler?.UpdateVdc32Channels(result.Voltages, result.Alarms);
                return true;
            }

            if (!result.IsCancelled)
            {
                _logAction($"读取数据失败: {result.ErrorMessage}", false);
            }

            return false;
        }

        /// <summary>
        /// 更新设备信息显示
        /// </summary>
        public void UpdateDeviceInfo(string version, string name, byte address)
        {
            if (_view == null) return;

            _view.FirmwareVersion = $"固件版本: {version}";
            _view.DeviceName = $"设备名称: {name}";
            _view.SlaveAddress = $"从机地址: {address}";

            _logAction($"设备信息 - 版本: {version}, 名称: {name}, 地址: {address}", true);
        }

        /// <summary>
        /// 更新环境数据显示
        /// </summary>
        public void UpdateEnvironmentData(double temperature, double humidity)
        {
            if (_view == null) return;

            _view.Temperature = $"{temperature:F1} ℃";
            _view.Humidity = $"{humidity:F1} %";
        }

        /// <summary>
        /// 更新 IO 状态显示
        /// </summary>
        public void UpdateIoStatus(bool[] ioStates)
        {
            _view?.UpdateIoStatus(ioStates);
        }

        #endregion

        #region 私有方法

        private void ValidateConfig(Vdc32PresenterConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (config.View == null)
                throw new ArgumentNullException(nameof(config.View));
            if (config.PollingCoordinator == null)
                throw new ArgumentNullException(nameof(config.PollingCoordinator));
            if (config.DataReadHandler == null)
                throw new ArgumentNullException(nameof(config.DataReadHandler));
            if (config.DisplayHandler == null)
                throw new ArgumentNullException(nameof(config.DisplayHandler));
        }

        private void BindEvents()
        {
            _pollingCoordinator.PollingCycleCompleted += OnPollingCycleCompleted;

            if (_view != null)
            {
                _view.RefreshRequested += OnRefreshRequested;
                _view.ExportRequested += OnExportRequested;
            }
        }

        private void UnbindEvents()
        {
            _pollingCoordinator.PollingCycleCompleted -= OnPollingCycleCompleted;

            if (_view != null)
            {
                _view.RefreshRequested -= OnRefreshRequested;
                _view.ExportRequested -= OnExportRequested;
            }
        }

        private void OnPollingCycleCompleted(object sender, PollingDataEventArgs e)
        {
            // 轮询完成后的处理（如果需要）
        }

        private async void OnRefreshRequested(object sender, EventArgs e)
        {
            await _pollingCoordinator.PauseAndExecuteAsync(async () =>
            {
                await ReadDataOnceAsync(CancellationToken.None);
            });
        }

        private void OnExportRequested(object sender, EventArgs e)
        {
            // 触发导出逻辑
            _logAction("导出数据...", null);
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
    /// VDC-32 Presenter 配置
    /// </summary>
    public class Vdc32PresenterConfig
    {
        public IVdc32View View { get; set; }
        public IPollingCoordinator PollingCoordinator { get; set; }
        public DataReadHandler DataReadHandler { get; set; }
        public ChannelDisplayHandler DisplayHandler { get; set; }
        public Action<string, bool?> LogAction { get; set; }
    }
}
