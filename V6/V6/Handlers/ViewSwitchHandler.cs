using System;
using System.Drawing;
using System.Windows.Forms;

namespace GJVdc32Tool.Handlers
{
    /// <summary>
    /// 视图切换处理器
    /// 职责：处理主界面的视图切换逻辑
    /// </summary>
    public class ViewSwitchHandler
    {
        #region 常量定义

        public const string VIEW_VDC32 = "VDC32";
        public const string VIEW_LOAD = "LOAD";
        public const string VIEW_LOG = "LOG";

        private static readonly Color ACTIVE_MENU_COLOR = Color.FromArgb(62, 62, 66);
        private static readonly Color NORMAL_MENU_COLOR = Color.FromArgb(45, 45, 48);

        #endregion

        #region 私有字段

        private readonly Control _vdc32Panel;
        private readonly Control _loadPanel;
        private readonly Control _logPanel;
        private readonly Control _connectionPanel;
        private readonly Button _vdc32MenuButton;
        private readonly Button _loadMenuButton;
        private readonly Button _logMenuButton;
        private readonly Action<string> _onViewChanged;

        private string _currentView = VIEW_VDC32;

        #endregion

        #region 属性

        /// <summary>
        /// 当前视图
        /// </summary>
        public string CurrentView => _currentView;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建视图切换处理器
        /// </summary>
        public ViewSwitchHandler(ViewSwitchConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _vdc32Panel = config.Vdc32Panel;
            _loadPanel = config.LoadPanel;
            _logPanel = config.LogPanel;
            _connectionPanel = config.ConnectionPanel;
            _vdc32MenuButton = config.Vdc32MenuButton;
            _loadMenuButton = config.LoadMenuButton;
            _logMenuButton = config.LogMenuButton;
            _onViewChanged = config.OnViewChanged;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 切换到指定视图
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns>是否切换成功</returns>
        public bool SwitchTo(string viewName)
        {
            if (string.IsNullOrWhiteSpace(viewName))
                return false;

            if (viewName == _currentView)
                return true;

            // 更新菜单状态
            UpdateMenuButtonState(_currentView, false);
            UpdateMenuButtonState(viewName, true);

            // 切换面板可见性
            switch (viewName)
            {
                case VIEW_VDC32:
                    ShowVdc32View();
                    break;
                case VIEW_LOAD:
                    ShowLoadView();
                    break;
                case VIEW_LOG:
                    ShowLogView();
                    break;
                default:
                    return false;
            }

            _currentView = viewName;
            _onViewChanged?.Invoke(viewName);

            return true;
        }

        /// <summary>
        /// 切换到 VDC-32 视图
        /// </summary>
        public void SwitchToVdc32() => SwitchTo(VIEW_VDC32);

        /// <summary>
        /// 切换到负载设备视图
        /// </summary>
        public void SwitchToLoad() => SwitchTo(VIEW_LOAD);

        /// <summary>
        /// 切换到日志视图
        /// </summary>
        public void SwitchToLog() => SwitchTo(VIEW_LOG);

        /// <summary>
        /// 获取当前视图的设备类型
        /// </summary>
        public Interfaces.DeviceType GetCurrentDeviceType()
        {
            switch (_currentView)
            {
                case VIEW_VDC32:
                    return Interfaces.DeviceType.VDC32;
                case VIEW_LOAD:
                    return Interfaces.DeviceType.LoadDevice;
                default:
                    return Interfaces.DeviceType.None;
            }
        }

        #endregion

        #region 私有方法

        private void ShowVdc32View()
        {
            SetPanelVisibility(_vdc32Panel, true);
            SetPanelVisibility(_loadPanel, false);
            SetPanelVisibility(_logPanel, false);
            SetPanelVisibility(_connectionPanel, true);
        }

        private void ShowLoadView()
        {
            SetPanelVisibility(_vdc32Panel, false);
            SetPanelVisibility(_loadPanel, true);
            SetPanelVisibility(_logPanel, false);
            SetPanelVisibility(_connectionPanel, true);
        }

        private void ShowLogView()
        {
            SetPanelVisibility(_vdc32Panel, false);
            SetPanelVisibility(_loadPanel, false);
            SetPanelVisibility(_logPanel, true);
            SetPanelVisibility(_connectionPanel, false);
        }

        private void SetPanelVisibility(Control panel, bool visible)
        {
            if (panel != null)
            {
                panel.Visible = visible;
            }
        }

        private void UpdateMenuButtonState(string viewName, bool isActive)
        {
            Button button = GetMenuButton(viewName);
            if (button != null)
            {
                button.BackColor = isActive ? ACTIVE_MENU_COLOR : NORMAL_MENU_COLOR;
            }
        }

        private Button GetMenuButton(string viewName)
        {
            switch (viewName)
            {
                case VIEW_VDC32:
                    return _vdc32MenuButton;
                case VIEW_LOAD:
                    return _loadMenuButton;
                case VIEW_LOG:
                    return _logMenuButton;
                default:
                    return null;
            }
        }

        #endregion
    }

    /// <summary>
    /// 视图切换配置
    /// </summary>
    public class ViewSwitchConfig
    {
        public Control Vdc32Panel { get; set; }
        public Control LoadPanel { get; set; }
        public Control LogPanel { get; set; }
        public Control ConnectionPanel { get; set; }
        public Button Vdc32MenuButton { get; set; }
        public Button LoadMenuButton { get; set; }
        public Button LogMenuButton { get; set; }
        public Action<string> OnViewChanged { get; set; }
    }
}
