using System;
using System.Drawing;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Coordinators
{
    /// <summary>
    /// UI 状态协调器
    /// 职责：统一管理界面状态更新，避免 UI 逻辑分散在各处
    /// </summary>
    /// <remarks>
    /// 架构规则：
    /// - 不直接操作 Windows Forms 控件
    /// - 通过接口与视图层交互
    /// - 所有颜色使用常量定义
    /// </remarks>
    public class UIStateCoordinator : IUIStateCoordinator
    {
        #region 颜色常量

        /// <summary>
        /// 成功状态颜色（绿色）
        /// </summary>
        public static readonly Color COLOR_SUCCESS = Color.FromArgb(76, 175, 80);

        /// <summary>
        /// 错误状态颜色（红色）
        /// </summary>
        public static readonly Color COLOR_ERROR = Color.FromArgb(244, 67, 54);

        /// <summary>
        /// 警告状态颜色（橙色）
        /// </summary>
        public static readonly Color COLOR_WARNING = Color.FromArgb(255, 152, 0);

        /// <summary>
        /// 信息状态颜色（蓝色）
        /// </summary>
        public static readonly Color COLOR_INFO = Color.FromArgb(33, 150, 243);

        /// <summary>
        /// 禁用状态颜色（灰色）
        /// </summary>
        public static readonly Color COLOR_DISABLED = Color.FromArgb(158, 158, 158);

        /// <summary>
        /// 默认文本颜色（深灰）
        /// </summary>
        public static readonly Color COLOR_TEXT_DEFAULT = Color.FromArgb(66, 66, 66);

        #endregion

        #region 私有字段

        private readonly IMainView _mainView;
        private string _currentView = "VDC32";
        private bool _isLoading = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建 UI 状态协调器
        /// </summary>
        /// <param name="mainView">主视图接口</param>
        public UIStateCoordinator(IMainView mainView)
        {
            _mainView = mainView ?? throw new ArgumentNullException(nameof(mainView));
        }

        #endregion

        #region IUIStateCoordinator 实现

        /// <inheritdoc/>
        public void UpdateConnectionState(DeviceType deviceType, bool isConnected)
        {
            string statusText;
            Color buttonColor;
            string buttonText;

            if (isConnected)
            {
                statusText = "● 已连接";
                buttonColor = COLOR_SUCCESS;
                buttonText = "断开";
            }
            else
            {
                statusText = "● 未连接";
                buttonColor = COLOR_ERROR;
                buttonText = "连接";
            }

            // 更新连接面板
            if (_mainView.ConnectionPanel != null)
            {
                _mainView.ConnectionPanel.ConnectionStatusText = statusText;
                _mainView.ConnectionPanel.ConnectionStatusColor = isConnected ? COLOR_SUCCESS : COLOR_DISABLED;
                _mainView.ConnectionPanel.ConnectButtonText = buttonText;
                _mainView.ConnectionPanel.ConnectButtonColor = buttonColor;
            }

            // 根据设备类型更新相应控件
            switch (deviceType)
            {
                case DeviceType.VDC32:
                    EnableVdc32Controls(isConnected);
                    break;
                case DeviceType.LoadDevice:
                    EnableLoadDeviceControls(isConnected);
                    break;
            }

            // 启用/禁用连接配置控件
            EnableConnectionControls(!isConnected);

            OnUIStateChanged($"ConnectionState:{deviceType}:{isConnected}");
        }

        /// <inheritdoc/>
        public void UpdateConnectButton(string text, Color color, bool enabled = true)
        {
            if (_mainView.ConnectionPanel != null)
            {
                _mainView.ConnectionPanel.ConnectButtonText = text;
                _mainView.ConnectionPanel.ConnectButtonColor = color;
            }
        }

        /// <inheritdoc/>
        public void EnableVdc32Controls(bool enabled)
        {
            // 通过主视图接口控制 VDC-32 相关控件
            // 具体实现由 MainForm 完成
            OnUIStateChanged($"EnableVdc32Controls:{enabled}");
        }

        /// <inheritdoc/>
        public void EnableLoadDeviceControls(bool enabled)
        {
            if (_mainView.LoadDeviceView != null)
            {
                _mainView.LoadDeviceView.SingleChannelConfigEnabled = enabled;
                _mainView.LoadDeviceView.BatchConfigEnabled = enabled;
            }

            OnUIStateChanged($"EnableLoadDeviceControls:{enabled}");
        }

        /// <inheritdoc/>
        public void EnableConnectionControls(bool enabled)
        {
            if (_mainView.ConnectionPanel != null)
            {
                _mainView.ConnectionPanel.SerialControlsEnabled = enabled;
                _mainView.ConnectionPanel.TcpControlsEnabled = enabled;
                _mainView.ConnectionPanel.SerialOptionEnabled = enabled;
                
                // TCP 选项根据当前设备类型决定
                bool tcpAvailable = enabled && _currentView == "VDC32";
                _mainView.ConnectionPanel.TcpOptionEnabled = tcpAvailable;
            }

            OnUIStateChanged($"EnableConnectionControls:{enabled}");
        }

        /// <inheritdoc/>
        public void EnableAllOperationControls(bool enabled)
        {
            EnableVdc32Controls(enabled);
            EnableLoadDeviceControls(enabled);

            OnUIStateChanged($"EnableAllOperationControls:{enabled}");
        }

        /// <inheritdoc/>
        public void ResetAllStatusIndicators()
        {
            // 通知视图层重置所有状态指示器
            OnUIStateChanged("ResetAllStatusIndicators");
        }

        /// <inheritdoc/>
        public void UpdateDeviceInfo(string version, string name, string address)
        {
            if (_mainView.Vdc32View != null)
            {
                _mainView.Vdc32View.FirmwareVersion = $"固件版本: {version}";
                _mainView.Vdc32View.DeviceName = $"设备名称: {name}";
                _mainView.Vdc32View.SlaveAddress = $"从机地址: {address}";
            }

            OnUIStateChanged($"UpdateDeviceInfo:{version},{name},{address}");
        }

        /// <inheritdoc/>
        public void ClearDeviceInfo()
        {
            if (_mainView.Vdc32View != null)
            {
                _mainView.Vdc32View.FirmwareVersion = "固件版本: --";
                _mainView.Vdc32View.DeviceName = "设备名称: --";
                _mainView.Vdc32View.SlaveAddress = "从机地址: --";
            }

            OnUIStateChanged("ClearDeviceInfo");
        }

        /// <inheritdoc/>
        public void PrepareViewSwitch(string targetView)
        {
            // 验证视图名称
            if (targetView != "VDC32" && targetView != "LOAD" && targetView != "LOG")
            {
                throw new ArgumentException($"无效的视图名称: {targetView}", nameof(targetView));
            }

            // 更新菜单状态
            _mainView.UpdateMenuButtonState(_currentView, false);
            _mainView.UpdateMenuButtonState(targetView, true);

            // 根据目标视图调整连接面板
            if (targetView == "LOG")
            {
                // 日志视图不显示连接面板
            }
            else if (targetView == "LOAD")
            {
                // 负载设备强制串口模式
                if (_mainView.ConnectionPanel != null)
                {
                    _mainView.ConnectionPanel.TcpOptionEnabled = false;
                    _mainView.ConnectionPanel.PanelTitle = "● GJDD-750 连接配置 (仅串口)";
                }
            }
            else // VDC32
            {
                if (_mainView.ConnectionPanel != null)
                {
                    _mainView.ConnectionPanel.TcpOptionEnabled = true;
                    _mainView.ConnectionPanel.PanelTitle = "● VDC-32 连接配置";
                }
            }

            OnUIStateChanged($"PrepareViewSwitch:{targetView}");
        }

        /// <inheritdoc/>
        public void CompleteViewSwitch(string currentView)
        {
            _currentView = currentView;
            OnUIStateChanged($"CompleteViewSwitch:{currentView}");
        }

        /// <inheritdoc/>
        public void ShowLoading(string message)
        {
            _isLoading = true;
            _mainView.ShowStatus(message, null);
            OnUIStateChanged($"ShowLoading:{message}");
        }

        /// <inheritdoc/>
        public void HideLoading()
        {
            _isLoading = false;
            OnUIStateChanged("HideLoading");
        }

        /// <inheritdoc/>
        public void UpdateProgress(int progress, string message)
        {
            if (progress < 0 || progress > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(progress), "进度范围: 0-100");
            }

            _mainView.ShowStatus($"{message} ({progress}%)", null);
            OnUIStateChanged($"UpdateProgress:{progress}:{message}");
        }

        #endregion

        #region 事件

        /// <inheritdoc/>
        public event EventHandler<string> UIStateChanged;

        private void OnUIStateChanged(string stateInfo)
        {
            UIStateChanged?.Invoke(this, stateInfo);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取状态前缀图标
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <returns>状态图标</returns>
        public static string GetStatusPrefix(bool? success)
        {
            if (success == true) return "✓";
            if (success == false) return "✗";
            return "ℹ";
        }

        /// <summary>
        /// 获取状态颜色
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <returns>颜色</returns>
        public static Color GetStatusColor(bool? success)
        {
            if (success == true) return COLOR_SUCCESS;
            if (success == false) return COLOR_ERROR;
            return COLOR_TEXT_DEFAULT;
        }

        #endregion
    }
}
