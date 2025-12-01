using System;
using System.Threading.Tasks;
using GJVdc32Tool.Coordinators;
using GJVdc32Tool.Handlers;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Presenters
{
    /// <summary>
    /// 主窗体 Presenter
    /// 职责：协调主界面的所有业务逻辑，是 MVP 模式的核心控制器
    /// </summary>
    public class MainPresenter : IDisposable
    {
        #region 私有字段

        private readonly IMainView _view;
        private readonly IDeviceConnectionCoordinator _connectionCoordinator;
        private readonly IPollingCoordinator _pollingCoordinator;
        private readonly IUIStateCoordinator _uiStateCoordinator;

        private readonly ConnectionHandler _connectionHandler;
        private readonly ViewSwitchHandler _viewSwitchHandler;

        private readonly Vdc32Presenter _vdc32Presenter;
        private readonly LoadDevicePresenter _loadPresenter;

        private bool _disposed = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建主 Presenter
        /// </summary>
        public MainPresenter(MainPresenterConfig config)
        {
            ValidateConfig(config);

            _view = config.View;
            _connectionCoordinator = config.ConnectionCoordinator;
            _pollingCoordinator = config.PollingCoordinator;
            _uiStateCoordinator = config.UIStateCoordinator;

            // 创建处理器
            _connectionHandler = new ConnectionHandler(
                _connectionCoordinator,
                _pollingCoordinator,
                _uiStateCoordinator,
                AddLog
            );

            _viewSwitchHandler = config.ViewSwitchHandler;

            // 创建子 Presenter
            _vdc32Presenter = config.Vdc32Presenter;
            _loadPresenter = config.LoadPresenter;

            // 绑定事件
            BindEvents();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化 Presenter
        /// </summary>
        public void Initialize()
        {
            AddLog("GJVdc32Tool 启动", true);
            _uiStateCoordinator.EnableAllOperationControls(false);
            _viewSwitchHandler?.SwitchToVdc32();
        }

        /// <summary>
        /// 处理连接按钮点击
        /// </summary>
        public async Task HandleConnectClickAsync()
        {
            if (_connectionCoordinator.HasActiveConnection)
            {
                await HandleDisconnectAsync();
            }
            else
            {
                await HandleConnectAsync();
            }
        }

        /// <summary>
        /// 处理视图切换
        /// </summary>
        public async Task HandleViewSwitchAsync(string targetView)
        {
            // 如果有连接，先断开
            if (_connectionCoordinator.HasActiveConnection)
            {
                var result = await _connectionHandler.HandleDisconnectAsync();
                if (!result.Success)
                {
                    _view.ShowMessage(result.Message, "警告", false);
                }
            }

            _viewSwitchHandler?.SwitchTo(targetView);

            // 更新连接面板标题
            UpdateConnectionPanelForView(targetView);
        }

        /// <summary>
        /// 处理窗体关闭
        /// </summary>
        public async Task HandleFormClosingAsync()
        {
            // 停止轮询
            if (_pollingCoordinator.IsPolling)
            {
                await _pollingCoordinator.StopAsync(waitForComplete: true);
            }

            // 断开连接
            if (_connectionCoordinator.HasActiveConnection)
            {
                await _connectionCoordinator.DisconnectAllAsync();
            }

            AddLog("程序退出", true);
        }

        #endregion

        #region 私有方法 - 连接处理

        private async Task HandleConnectAsync()
        {
            var currentView = _viewSwitchHandler?.CurrentView ?? ViewSwitchHandler.VIEW_VDC32;
            ConnectionResult result;

            if (currentView == ViewSwitchHandler.VIEW_LOAD)
            {
                result = await ConnectLoadDeviceAsync();
            }
            else
            {
                result = await ConnectVdc32Async();
            }

            if (result.Success)
            {
                await StartPollingForCurrentDeviceAsync();
            }
            else
            {
                _view.ShowMessage(result.Message, "连接失败", false);
            }
        }

        private async Task<ConnectionResult> ConnectVdc32Async()
        {
            var panel = _view.ConnectionPanel;
            if (panel == null)
            {
                return ConnectionResult.Fail("连接面板未初始化");
            }

            bool useTcp = panel.IsTcpMode;

            if (useTcp)
            {
                return await _connectionHandler.HandleVdc32TcpConnectAsync(
                    panel.IpAddress,
                    panel.TcpPort,
                    panel.SlaveId
                );
            }
            else
            {
                return await _connectionHandler.HandleVdc32SerialConnectAsync(
                    panel.SelectedPort,
                    panel.BaudRate,
                    panel.SlaveId
                );
            }
        }

        private async Task<ConnectionResult> ConnectLoadDeviceAsync()
        {
            var panel = _view.ConnectionPanel;
            if (panel == null)
            {
                return ConnectionResult.Fail("连接面板未初始化");
            }

            return await _connectionHandler.HandleLoadDeviceConnectAsync(
                panel.SelectedPort,
                panel.BaudRate
            );
        }

        private async Task HandleDisconnectAsync()
        {
            var result = await _connectionHandler.HandleDisconnectAsync();

            if (!result.Success)
            {
                _view.ShowMessage(result.Message, "断开失败", false);
            }
        }

        private async Task StartPollingForCurrentDeviceAsync()
        {
            var deviceType = _connectionCoordinator.CurrentConnectedDevice;

            switch (deviceType)
            {
                case DeviceType.VDC32:
                    await _pollingCoordinator.StartVdc32PollingAsync();
                    _vdc32Presenter?.OnPollingStarted();
                    break;

                case DeviceType.LoadDevice:
                    await _pollingCoordinator.StartLoadDevicePollingAsync();
                    _loadPresenter?.OnPollingStarted();
                    break;
            }
        }

        #endregion

        #region 私有方法 - 事件绑定

        private void BindEvents()
        {
            // 连接状态变更
            _connectionCoordinator.ConnectionStateChanged += OnConnectionStateChanged;
            _connectionCoordinator.BeforeDisconnectOther += OnBeforeDisconnectOther;
            _connectionCoordinator.ConnectionFailed += OnConnectionFailed;

            // 轮询状态变更
            _pollingCoordinator.StateChanged += OnPollingStateChanged;
            _pollingCoordinator.PollingError += OnPollingError;
            _pollingCoordinator.ErrorThresholdReached += OnErrorThresholdReached;
        }

        private void UnbindEvents()
        {
            _connectionCoordinator.ConnectionStateChanged -= OnConnectionStateChanged;
            _connectionCoordinator.BeforeDisconnectOther -= OnBeforeDisconnectOther;
            _connectionCoordinator.ConnectionFailed -= OnConnectionFailed;

            _pollingCoordinator.StateChanged -= OnPollingStateChanged;
            _pollingCoordinator.PollingError -= OnPollingError;
            _pollingCoordinator.ErrorThresholdReached -= OnErrorThresholdReached;
        }

        #endregion

        #region 私有方法 - 事件处理

        private void OnConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            _uiStateCoordinator.UpdateConnectionState(e.DeviceType, e.IsConnected);

            if (!e.IsConnected)
            {
                // 断开时重置显示
                switch (e.DeviceType)
                {
                    case DeviceType.VDC32:
                        _vdc32Presenter?.ResetDisplay();
                        break;
                    case DeviceType.LoadDevice:
                        _loadPresenter?.ResetDisplay();
                        break;
                }
            }
        }

        private void OnBeforeDisconnectOther(object sender, DeviceType deviceType)
        {
            string deviceName = deviceType == DeviceType.VDC32 ? "VDC-32" : "GJDD-750";
            AddLog($"正在断开 {deviceName} 以连接新设备...", null);
        }

        private void OnConnectionFailed(object sender, Exception ex)
        {
            AddLog($"连接失败: {ex.Message}", false);
        }

        private void OnPollingStateChanged(object sender, PollingState state)
        {
            string stateText = GetPollingStateText(state);
            _view.ShowStatus($"轮询: {stateText}", null);
        }

        private void OnPollingError(object sender, Exception ex)
        {
            AddLog($"轮询错误: {ex.Message}", false);
        }

        private async void OnErrorThresholdReached(object sender, int errorCount)
        {
            AddLog($"连续 {errorCount} 次轮询错误，已停止轮询", false);
            await _connectionHandler.HandleDisconnectAsync();
        }

        #endregion

        #region 私有方法 - 辅助

        private void ValidateConfig(MainPresenterConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (config.View == null)
                throw new ArgumentNullException(nameof(config.View));
            if (config.ConnectionCoordinator == null)
                throw new ArgumentNullException(nameof(config.ConnectionCoordinator));
            if (config.PollingCoordinator == null)
                throw new ArgumentNullException(nameof(config.PollingCoordinator));
            if (config.UIStateCoordinator == null)
                throw new ArgumentNullException(nameof(config.UIStateCoordinator));
        }

        private void UpdateConnectionPanelForView(string viewName)
        {
            if (_view.ConnectionPanel == null) return;

            switch (viewName)
            {
                case ViewSwitchHandler.VIEW_VDC32:
                    _view.ConnectionPanel.PanelTitle = "● VDC-32 连接配置";
                    _view.ConnectionPanel.TcpOptionEnabled = true;
                    _view.ConnectionPanel.SlaveIdVisible = true;
                    break;

                case ViewSwitchHandler.VIEW_LOAD:
                    _view.ConnectionPanel.PanelTitle = "● GJDD-750 连接配置 (仅串口)";
                    _view.ConnectionPanel.TcpOptionEnabled = false;
                    _view.ConnectionPanel.SlaveIdVisible = false;
                    break;

                case ViewSwitchHandler.VIEW_LOG:
                    // 日志视图不需要连接面板
                    break;
            }
        }

        private string GetPollingStateText(PollingState state)
        {
            switch (state)
            {
                case PollingState.Running: return "运行中";
                case PollingState.Paused: return "已暂停";
                case PollingState.Stopping: return "正在停止";
                case PollingState.Stopped: return "已停止";
                default: return "未知";
            }
        }

        private void AddLog(string message, bool? success)
        {
            _view.AddLog(message, success);
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
                _vdc32Presenter?.Dispose();
                _loadPresenter?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }

    /// <summary>
    /// MainPresenter 配置
    /// </summary>
    public class MainPresenterConfig
    {
        public IMainView View { get; set; }
        public IDeviceConnectionCoordinator ConnectionCoordinator { get; set; }
        public IPollingCoordinator PollingCoordinator { get; set; }
        public IUIStateCoordinator UIStateCoordinator { get; set; }
        public ViewSwitchHandler ViewSwitchHandler { get; set; }
        public Vdc32Presenter Vdc32Presenter { get; set; }
        public LoadDevicePresenter LoadPresenter { get; set; }
    }
}
