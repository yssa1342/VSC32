using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GJVdc32Tool.Builders;
using GJVdc32Tool.Coordinators;
using GJVdc32Tool.Handlers;
using GJVdc32Tool.Interfaces;
using GJVdc32Tool.Presenters;
using GJVdc32Tool.Views;

namespace GJVdc32Tool
{
    /// <summary>
    /// 主窗体 - 重构版
    /// 职责：仅负责 UI 布局和事件路由，业务逻辑委托给 Presenter
    /// </summary>
    public partial class MainForm : Form, IMainView
    {
        #region 私有字段 - 视图控件

        private Panel _menuPanel;
        private Panel _contentPanel;
        private Panel _rightPanel;
        private Panel _statusBarPanel;

        private Vdc32ChannelView _vdc32View;
        private LoadDeviceView _loadView;
        private ConnectionPanelView _connectionPanel;
        private LogView _logView;

        private Button _btnMenuVdc32;
        private Button _btnMenuLoad;
        private Button _btnMenuLog;

        #endregion

        #region 私有字段 - Presenter 和 Coordinator

        private MainPresenter _presenter;
        private DeviceConnectionCoordinator _connectionCoordinator;
        private PollingCoordinator _pollingCoordinator;
        private UIStateCoordinator _uiStateCoordinator;

        #endregion

        #region IMainView 实现

        public IVdc32View Vdc32View => _vdc32View;
        public ILoadDeviceView LoadDeviceView => _loadView;
        public IConnectionPanel ConnectionPanel => _connectionPanel;

        public void ShowStatus(string message, bool? success)
        {
            // 状态栏更新
            InvokeIfRequired(() =>
            {
                // 更新状态栏标签（如果有）
            });
        }

        public void ShowMessage(string message, string title, bool isError)
        {
            InvokeIfRequired(() =>
            {
                MessageBox.Show(
                    this,
                    message,
                    title,
                    MessageBoxButtons.OK,
                    isError ? MessageBoxIcon.Error : MessageBoxIcon.Information
                );
            });
        }

        public void AddLog(string message, bool? success)
        {
            _logView?.AddLog(message, success);
        }

        public void UpdateMenuButtonState(string viewName, bool isActive)
        {
            InvokeIfRequired(() =>
            {
                var activeColor = Color.FromArgb(62, 62, 66);
                var normalColor = Color.FromArgb(45, 45, 48);

                switch (viewName)
                {
                    case "VDC32":
                        if (_btnMenuVdc32 != null)
                            _btnMenuVdc32.BackColor = isActive ? activeColor : normalColor;
                        break;
                    case "LOAD":
                        if (_btnMenuLoad != null)
                            _btnMenuLoad.BackColor = isActive ? activeColor : normalColor;
                        break;
                    case "LOG":
                        if (_btnMenuLog != null)
                            _btnMenuLog.BackColor = isActive ? activeColor : normalColor;
                        break;
                }
            });
        }

        #endregion

        #region 构造函数

        public MainForm()
        {
            InitializeComponent();
            BuildLayout();
            InitializeCoordinators();
            InitializePresenter();
            BindEvents();
        }

        #endregion

        #region 初始化方法

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1280, 720);
            this.MinimumSize = new Size(1024, 600);
            this.Name = "MainForm";
            this.Text = "GJVdc32Tool - VDC-32 & GJDD-750 调试工具";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.ResumeLayout(false);
        }

        private void BuildLayout()
        {
            this.SuspendLayout();

            // 左侧菜单面板
            _menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 180,
                BackColor = Color.FromArgb(45, 45, 48)
            };
            this.Controls.Add(_menuPanel);
            BuildMenu();

            // 底部状态栏
            _statusBarPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                BackColor = Color.FromArgb(0, 122, 204)
            };
            this.Controls.Add(_statusBarPanel);
            BuildStatusBar();

            // 右侧面板（连接配置）
            _rightPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 300,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10)
            };
            this.Controls.Add(_rightPanel);

            // 连接配置控件
            _connectionPanel = new ConnectionPanelView
            {
                Dock = DockStyle.Top
            };
            _rightPanel.Controls.Add(_connectionPanel);

            // 日志控件（在连接面板下方）
            _logView = new LogView
            {
                Dock = DockStyle.Fill
            };
            _rightPanel.Controls.Add(_logView);
            _logView.BringToFront();

            // 中间内容区域
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10)
            };
            this.Controls.Add(_contentPanel);

            // VDC-32 视图
            _vdc32View = new Vdc32ChannelView
            {
                Dock = DockStyle.Fill
            };
            _contentPanel.Controls.Add(_vdc32View);

            // 负载设备视图
            _loadView = new LoadDeviceView
            {
                Dock = DockStyle.Fill,
                Visible = false
            };
            _contentPanel.Controls.Add(_loadView);

            this.ResumeLayout(true);
        }

        private void BuildMenu()
        {
            var menuBuilder = new MenuBuilder(_menuPanel)
                .AddItem("VDC32", "VDC-32 电压检测", "📊")
                .AddItem("LOAD", "GJDD-750 负载", "⚡")
                .AddItem("LOG", "运行日志", "📋")
                .WithActiveBackColor(Color.FromArgb(62, 62, 66));

            var result = menuBuilder.Build();

            if (result.Success)
            {
                result.MenuButtons.TryGetValue("VDC32", out _btnMenuVdc32);
                result.MenuButtons.TryGetValue("LOAD", out _btnMenuLoad);
                result.MenuButtons.TryGetValue("LOG", out _btnMenuLog);
            }

            // Logo/Title
            var titleLabel = new Label
            {
                Text = "GJVdc32Tool",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _menuPanel.Controls.Add(titleLabel);
        }

        private void BuildStatusBar()
        {
            var statusLabel = new Label
            {
                Text = "就绪",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.White,
                Location = new Point(10, 5),
                AutoSize = true
            };
            _statusBarPanel.Controls.Add(statusLabel);

            var versionLabel = new Label
            {
                Text = "v2.0.0",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.White,
                Anchor = AnchorStyles.Right,
                AutoSize = true
            };
            versionLabel.Location = new Point(_statusBarPanel.Width - 60, 5);
            _statusBarPanel.Controls.Add(versionLabel);
        }

        private void InitializeCoordinators()
        {
            // 注意：这里需要实际的设备服务委托
            // 在实际使用时，需要替换为真实的 DeviceService 和 LoadDeviceService

            _connectionCoordinator = new DeviceConnectionCoordinator(
                connectVdc32Serial: (port, baud, slave) => 
                {
                    // TODO: 替换为 _deviceService.ConnectSerial(port, baud, slave)
                    AddLog($"模拟串口连接: {port}", true);
                    return true;
                },
                connectVdc32Tcp: async (ip, port, slave, timeout) =>
                {
                    // TODO: 替换为 _deviceService.ConnectTcpAsync(ip, port, slave, timeout)
                    await Task.Delay(100);
                    AddLog($"模拟 TCP 连接: {ip}:{port}", true);
                    return true;
                },
                connectLoadDevice: (port, baud) =>
                {
                    // TODO: 替换为 _loadService.Connect(port, baud)
                    AddLog($"模拟负载设备连接: {port}", true);
                    return true;
                },
                disconnectVdc32: async () =>
                {
                    // TODO: 替换为实际断开逻辑
                    await Task.Delay(50);
                    AddLog("VDC-32 已断开", true);
                },
                disconnectLoadDevice: async () =>
                {
                    // TODO: 替换为实际断开逻辑
                    await Task.Delay(50);
                    AddLog("GJDD-750 已断开", true);
                },
                isVdc32Connected: () => false,  // TODO: 替换为 _deviceService.IsConnected
                isLoadDeviceConnected: () => false  // TODO: 替换为 _loadService.IsConnected
            );

            _pollingCoordinator = new PollingCoordinator(
                vdc32PollingAction: async (token) =>
                {
                    // TODO: 替换为实际轮询逻辑
                    await Task.Delay(100, token);
                    return true;
                },
                loadDevicePollingAction: async (token) =>
                {
                    // TODO: 替换为实际轮询逻辑
                    await Task.Delay(100, token);
                    return true;
                }
            );

            _uiStateCoordinator = new UIStateCoordinator(this);
        }

        private void InitializePresenter()
        {
            var viewSwitchHandler = new ViewSwitchHandler(new ViewSwitchConfig
            {
                Vdc32Panel = _vdc32View,
                LoadPanel = _loadView,
                LogPanel = null,  // 日志在右侧面板
                ConnectionPanel = _connectionPanel,
                Vdc32MenuButton = _btnMenuVdc32,
                LoadMenuButton = _btnMenuLoad,
                LogMenuButton = _btnMenuLog,
                OnViewChanged = OnViewChanged
            });

            // 创建 Handler
            var displayHandler = new ChannelDisplayHandler(this);
            displayHandler.ConfigureVdc32Display(_vdc32View.VoltageLabels, _vdc32View.IndicatorPanels);
            displayHandler.ConfigureLoadDisplay(
                _loadView.CurrentLabels,
                _loadView.PowerLabels,
                _loadView.StatusIndicators,
                _loadView.ToggleButtons
            );

            var dataReadHandler = new DataReadHandler(
                readVdc32Registers: async (token) =>
                {
                    // TODO: 替换为实际读取
                    await Task.Delay(50, token);
                    return new ushort[32];
                },
                readLoadChannels: async (token) =>
                {
                    // TODO: 替换为实际读取
                    await Task.Delay(50, token);
                    return new Handlers.LoadChannelData[8];
                },
                logAction: AddLog
            );

            var channelHandler = new LoadChannelHandler(
                setChannelState: async (ch, state) =>
                {
                    await Task.Delay(50);
                    return true;
                },
                setChannelCurrent: async (ch, current) =>
                {
                    await Task.Delay(50);
                    return true;
                },
                setAllChannelsCurrent: async (current) =>
                {
                    await Task.Delay(50);
                    return true;
                },
                setAllChannelsState: async (state) =>
                {
                    await Task.Delay(50);
                    return true;
                },
                logAction: AddLog
            );

            // 创建子 Presenter
            var vdc32Presenter = new Vdc32Presenter(new Vdc32PresenterConfig
            {
                View = _vdc32View,
                PollingCoordinator = _pollingCoordinator,
                DataReadHandler = dataReadHandler,
                DisplayHandler = displayHandler,
                LogAction = AddLog
            });

            var loadPresenter = new LoadDevicePresenter(new LoadDevicePresenterConfig
            {
                View = _loadView,
                PollingCoordinator = _pollingCoordinator,
                DataReadHandler = dataReadHandler,
                ChannelHandler = channelHandler,
                DisplayHandler = displayHandler,
                LogAction = AddLog
            });

            // 创建主 Presenter
            _presenter = new MainPresenter(new MainPresenterConfig
            {
                View = this,
                ConnectionCoordinator = _connectionCoordinator,
                PollingCoordinator = _pollingCoordinator,
                UIStateCoordinator = _uiStateCoordinator,
                ViewSwitchHandler = viewSwitchHandler,
                Vdc32Presenter = vdc32Presenter,
                LoadPresenter = loadPresenter
            });

            _presenter.Initialize();
        }

        private void BindEvents()
        {
            // 连接按钮事件
            _connectionPanel.ConnectRequested += async (s, e) =>
            {
                await _presenter.HandleConnectClickAsync();
            };

            // 菜单按钮事件
            if (_btnMenuVdc32 != null)
            {
                _btnMenuVdc32.Click += async (s, e) =>
                {
                    await _presenter.HandleViewSwitchAsync("VDC32");
                };
            }

            if (_btnMenuLoad != null)
            {
                _btnMenuLoad.Click += async (s, e) =>
                {
                    await _presenter.HandleViewSwitchAsync("LOAD");
                };
            }

            if (_btnMenuLog != null)
            {
                _btnMenuLog.Click += (s, e) =>
                {
                    // 日志视图切换（日志在右侧面板始终可见）
                    UpdateMenuButtonState("VDC32", false);
                    UpdateMenuButtonState("LOAD", false);
                    UpdateMenuButtonState("LOG", true);
                };
            }

            // 窗体关闭事件
            this.FormClosing += async (s, e) =>
            {
                e.Cancel = true;
                await _presenter.HandleFormClosingAsync();
                e.Cancel = false;
                this.Dispose();
            };
        }

        #endregion

        #region 私有方法

        private void OnViewChanged(string viewName)
        {
            AddLog($"切换到 {viewName} 视图", null);
        }

        private void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        #endregion

        #region 资源释放

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _presenter?.Dispose();
                _pollingCoordinator?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
