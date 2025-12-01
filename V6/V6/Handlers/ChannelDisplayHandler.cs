using System;
using System.Drawing;
using System.Windows.Forms;

namespace GJVdc32Tool.Handlers
{
    /// <summary>
    /// 通道显示更新处理器
    /// 职责：将数据模型更新到 UI 控件
    /// </summary>
    public class ChannelDisplayHandler
    {
        #region 常量定义

        private static readonly Color COLOR_NORMAL = Color.FromArgb(76, 175, 80);
        private static readonly Color COLOR_ALARM = Color.FromArgb(244, 67, 54);
        private static readonly Color COLOR_OFFLINE = Color.FromArgb(158, 158, 158);
        private static readonly Color COLOR_TEXT_NORMAL = Color.FromArgb(66, 66, 66);
        private static readonly Color COLOR_TEXT_ALARM = Color.FromArgb(244, 67, 54);

        private const string VOLTAGE_FORMAT = "{0:F2} V";
        private const string VOLTAGE_OFFLINE = "--.- V";
        private const string CURRENT_FORMAT = "{0:F2} A";
        private const string POWER_FORMAT = "{0:F1} W";

        #endregion

        #region 私有字段

        private readonly Control _parentControl;
        private Label[] _voltageLabels;
        private Panel[] _indicatorPanels;
        private Label[] _currentLabels;
        private Label[] _powerLabels;
        private Panel[] _loadIndicators;
        private Button[] _toggleButtons;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建通道显示处理器
        /// </summary>
        /// <param name="parentControl">父控件（用于 Invoke）</param>
        public ChannelDisplayHandler(Control parentControl)
        {
            _parentControl = parentControl ?? throw new ArgumentNullException(nameof(parentControl));
        }

        #endregion

        #region 配置方法

        /// <summary>
        /// 配置 VDC-32 显示控件
        /// </summary>
        public void ConfigureVdc32Display(Label[] voltageLabels, Panel[] indicatorPanels)
        {
            _voltageLabels = voltageLabels;
            _indicatorPanels = indicatorPanels;
        }

        /// <summary>
        /// 配置负载设备显示控件
        /// </summary>
        public void ConfigureLoadDisplay(
            Label[] currentLabels,
            Label[] powerLabels,
            Panel[] loadIndicators,
            Button[] toggleButtons)
        {
            _currentLabels = currentLabels;
            _powerLabels = powerLabels;
            _loadIndicators = loadIndicators;
            _toggleButtons = toggleButtons;
        }

        #endregion

        #region VDC-32 显示更新

        /// <summary>
        /// 更新 VDC-32 所有通道显示
        /// </summary>
        public void UpdateVdc32Channels(double[] voltages, bool[] alarms)
        {
            if (_voltageLabels == null || _indicatorPanels == null)
                return;

            if (voltages == null || alarms == null)
                return;

            InvokeIfRequired(() =>
            {
                int count = Math.Min(voltages.Length, _voltageLabels.Length);
                for (int i = 0; i < count; i++)
                {
                    UpdateVdc32Channel(i, voltages[i], alarms[i]);
                }
            });
        }

        /// <summary>
        /// 更新单个 VDC-32 通道显示
        /// </summary>
        public void UpdateVdc32Channel(int channelIndex, double voltage, bool isAlarm)
        {
            if (!ValidateVdc32Index(channelIndex))
                return;

            InvokeIfRequired(() =>
            {
                // 更新电压显示
                _voltageLabels[channelIndex].Text = string.Format(VOLTAGE_FORMAT, voltage);
                _voltageLabels[channelIndex].ForeColor = isAlarm ? COLOR_TEXT_ALARM : COLOR_TEXT_NORMAL;

                // 更新状态指示器
                _indicatorPanels[channelIndex].BackColor = isAlarm ? COLOR_ALARM : COLOR_NORMAL;
            });
        }

        /// <summary>
        /// 重置 VDC-32 所有通道为离线状态
        /// </summary>
        public void ResetVdc32Channels()
        {
            if (_voltageLabels == null || _indicatorPanels == null)
                return;

            InvokeIfRequired(() =>
            {
                for (int i = 0; i < _voltageLabels.Length; i++)
                {
                    _voltageLabels[i].Text = VOLTAGE_OFFLINE;
                    _voltageLabels[i].ForeColor = COLOR_TEXT_NORMAL;
                    _indicatorPanels[i].BackColor = COLOR_OFFLINE;
                }
            });
        }

        #endregion

        #region 负载设备显示更新

        /// <summary>
        /// 更新负载设备所有通道显示
        /// </summary>
        public void UpdateLoadChannels(LoadChannelData[] channels)
        {
            if (channels == null)
                return;

            InvokeIfRequired(() =>
            {
                for (int i = 0; i < channels.Length; i++)
                {
                    UpdateLoadChannel(i, channels[i]);
                }
            });
        }

        /// <summary>
        /// 更新单个负载通道显示
        /// </summary>
        public void UpdateLoadChannel(int channelIndex, LoadChannelData data)
        {
            if (!ValidateLoadIndex(channelIndex))
                return;

            if (data == null)
                return;

            InvokeIfRequired(() =>
            {
                // 更新电流显示
                if (_currentLabels != null && channelIndex < _currentLabels.Length)
                {
                    _currentLabels[channelIndex].Text = string.Format(CURRENT_FORMAT, data.Current);
                }

                // 更新功率显示
                if (_powerLabels != null && channelIndex < _powerLabels.Length)
                {
                    _powerLabels[channelIndex].Text = string.Format(POWER_FORMAT, data.Power);
                }

                // 更新状态指示器
                if (_loadIndicators != null && channelIndex < _loadIndicators.Length)
                {
                    Color indicatorColor = GetLoadIndicatorColor(data);
                    _loadIndicators[channelIndex].BackColor = indicatorColor;
                }

                // 更新开关按钮
                if (_toggleButtons != null && channelIndex < _toggleButtons.Length)
                {
                    UpdateToggleButton(_toggleButtons[channelIndex], data.IsOn);
                }
            });
        }

        /// <summary>
        /// 重置负载设备所有通道为离线状态
        /// </summary>
        public void ResetLoadChannels()
        {
            InvokeIfRequired(() =>
            {
                if (_currentLabels != null)
                {
                    foreach (var label in _currentLabels)
                    {
                        label.Text = "--.- A";
                    }
                }

                if (_powerLabels != null)
                {
                    foreach (var label in _powerLabels)
                    {
                        label.Text = "--.- W";
                    }
                }

                if (_loadIndicators != null)
                {
                    foreach (var indicator in _loadIndicators)
                    {
                        indicator.BackColor = COLOR_OFFLINE;
                    }
                }

                if (_toggleButtons != null)
                {
                    foreach (var btn in _toggleButtons)
                    {
                        btn.Text = "开启";
                        btn.BackColor = COLOR_OFFLINE;
                    }
                }
            });
        }

        #endregion

        #region 私有方法

        private bool ValidateVdc32Index(int index)
        {
            return _voltageLabels != null &&
                   _indicatorPanels != null &&
                   index >= 0 &&
                   index < _voltageLabels.Length &&
                   index < _indicatorPanels.Length;
        }

        private bool ValidateLoadIndex(int index)
        {
            return index >= 0 && index < 8;
        }

        private Color GetLoadIndicatorColor(LoadChannelData data)
        {
            if (data.HasFault)
                return COLOR_ALARM;
            if (data.IsOn)
                return COLOR_NORMAL;
            return COLOR_OFFLINE;
        }

        private void UpdateToggleButton(Button button, bool isOn)
        {
            button.Text = isOn ? "关闭" : "开启";
            button.BackColor = isOn ? COLOR_NORMAL : COLOR_OFFLINE;
        }

        private void InvokeIfRequired(Action action)
        {
            if (_parentControl.InvokeRequired)
            {
                _parentControl.Invoke(action);
            }
            else
            {
                action();
            }
        }

        #endregion
    }
}
