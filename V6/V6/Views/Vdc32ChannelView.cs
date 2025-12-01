using System;
using System.Drawing;
using System.Windows.Forms;
using GJVdc32Tool.Builders;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Views
{
    /// <summary>
    /// VDC-32 通道显示控件
    /// 职责：显示 32 通道电压数据和状态
    /// </summary>
    public partial class Vdc32ChannelView : UserControl, IVdc32View
    {
        #region 常量定义

        private const int CHANNEL_COUNT = 32;

        #endregion

        #region 私有字段

        private Label[] _voltageLabels;
        private Panel[] _indicatorPanels;
        private Label[] _channelLabels;

        private Label _lblFirmwareVersion;
        private Label _lblDeviceName;
        private Label _lblSlaveAddress;
        private Label _lblTemperature;
        private Label _lblHumidity;
        private Panel _ioStatusPanel;
        private Panel[] _ioIndicators;

        #endregion

        #region 事件

        public event EventHandler RefreshRequested;
        public event EventHandler ExportRequested;

        #endregion

        #region 属性

        public string FirmwareVersion
        {
            get => _lblFirmwareVersion?.Text ?? string.Empty;
            set { if (_lblFirmwareVersion != null) _lblFirmwareVersion.Text = value; }
        }

        public string DeviceName
        {
            get => _lblDeviceName?.Text ?? string.Empty;
            set { if (_lblDeviceName != null) _lblDeviceName.Text = value; }
        }

        public string SlaveAddress
        {
            get => _lblSlaveAddress?.Text ?? string.Empty;
            set { if (_lblSlaveAddress != null) _lblSlaveAddress.Text = value; }
        }

        public string Temperature
        {
            get => _lblTemperature?.Text ?? string.Empty;
            set { if (_lblTemperature != null) _lblTemperature.Text = value; }
        }

        public string Humidity
        {
            get => _lblHumidity?.Text ?? string.Empty;
            set { if (_lblHumidity != null) _lblHumidity.Text = value; }
        }

        /// <summary>
        /// 电压标签数组（供 Handler 使用）
        /// </summary>
        public Label[] VoltageLabels => _voltageLabels;

        /// <summary>
        /// 状态指示器数组（供 Handler 使用）
        /// </summary>
        public Panel[] IndicatorPanels => _indicatorPanels;

        #endregion

        #region 构造函数

        public Vdc32ChannelView()
        {
            InitializeComponent();
            BuildUI();
        }

        #endregion

        #region IVdc32View 实现

        public void UpdateChannelVoltage(int channelIndex, double voltage, bool isAlarm)
        {
            if (channelIndex < 0 || channelIndex >= CHANNEL_COUNT)
                return;

            InvokeIfRequired(() =>
            {
                _voltageLabels[channelIndex].Text = $"{voltage:F2} V";
                _voltageLabels[channelIndex].ForeColor = isAlarm 
                    ? Color.FromArgb(244, 67, 54) 
                    : Color.FromArgb(66, 66, 66);
                _indicatorPanels[channelIndex].BackColor = isAlarm
                    ? Color.FromArgb(244, 67, 54)
                    : Color.FromArgb(76, 175, 80);
            });
        }

        public void UpdateIoStatus(bool[] ioStates)
        {
            if (ioStates == null || _ioIndicators == null)
                return;

            InvokeIfRequired(() =>
            {
                int count = Math.Min(ioStates.Length, _ioIndicators.Length);
                for (int i = 0; i < count; i++)
                {
                    _ioIndicators[i].BackColor = ioStates[i]
                        ? Color.FromArgb(76, 175, 80)
                        : Color.FromArgb(158, 158, 158);
                }
            });
        }

        public void ResetAllChannels()
        {
            InvokeIfRequired(() =>
            {
                for (int i = 0; i < CHANNEL_COUNT; i++)
                {
                    _voltageLabels[i].Text = "--.- V";
                    _voltageLabels[i].ForeColor = Color.FromArgb(66, 66, 66);
                    _indicatorPanels[i].BackColor = Color.FromArgb(158, 158, 158);
                }

                FirmwareVersion = "固件版本: --";
                DeviceName = "设备名称: --";
                SlaveAddress = "从机地址: --";
                Temperature = "--.- ℃";
                Humidity = "--.- %";
            });
        }

        #endregion

        #region 私有方法

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Name = "Vdc32ChannelView";
            this.Size = new Size(1000, 500);
            this.ResumeLayout(false);
        }

        private void BuildUI()
        {
            this.SuspendLayout();

            // 设备信息区域
            var infoPanel = CreateDeviceInfoPanel();
            infoPanel.Location = new Point(10, 10);
            this.Controls.Add(infoPanel);

            // 通道显示区域
            var channelContainer = new Panel
            {
                Location = new Point(10, 90),
                Size = new Size(980, 350),
                AutoScroll = true
            };
            this.Controls.Add(channelContainer);

            // 使用 Builder 构建通道面板
            var builder = new ChannelPanelBuilder(channelContainer);
            var result = builder.Build();

            if (result.Success)
            {
                _voltageLabels = result.VoltageLabels;
                _indicatorPanels = result.IndicatorPanels;
                _channelLabels = result.ChannelLabels;
            }

            // IO 状态区域
            _ioStatusPanel = CreateIoStatusPanel();
            _ioStatusPanel.Location = new Point(10, 450);
            this.Controls.Add(_ioStatusPanel);

            this.ResumeLayout(true);
        }

        private Panel CreateDeviceInfoPanel()
        {
            var panel = new Panel
            {
                Size = new Size(980, 70),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var titleLabel = new Label
            {
                Text = "● 设备信息",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                Location = new Point(10, 5),
                AutoSize = true
            };
            panel.Controls.Add(titleLabel);

            // 固件版本
            _lblFirmwareVersion = CreateInfoLabel("固件版本: --", 10, 30);
            panel.Controls.Add(_lblFirmwareVersion);

            // 设备名称
            _lblDeviceName = CreateInfoLabel("设备名称: --", 180, 30);
            panel.Controls.Add(_lblDeviceName);

            // 从机地址
            _lblSlaveAddress = CreateInfoLabel("从机地址: --", 380, 30);
            panel.Controls.Add(_lblSlaveAddress);

            // 温度
            _lblTemperature = CreateInfoLabel("--.- ℃", 550, 30);
            panel.Controls.Add(_lblTemperature);

            // 湿度
            _lblHumidity = CreateInfoLabel("--.- %", 650, 30);
            panel.Controls.Add(_lblHumidity);

            // 刷新按钮
            var btnRefresh = new Button
            {
                Text = "刷新",
                Font = new Font("Segoe UI", 9f),
                Size = new Size(70, 28),
                Location = new Point(800, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnRefresh.Click += (s, e) => RefreshRequested?.Invoke(this, EventArgs.Empty);
            panel.Controls.Add(btnRefresh);

            // 导出按钮
            var btnExport = new Button
            {
                Text = "导出",
                Font = new Font("Segoe UI", 9f),
                Size = new Size(70, 28),
                Location = new Point(880, 28),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnExport.Click += (s, e) => ExportRequested?.Invoke(this, EventArgs.Empty);
            panel.Controls.Add(btnExport);

            return panel;
        }

        private Label CreateInfoLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(66, 66, 66),
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private Panel CreateIoStatusPanel()
        {
            var panel = new Panel
            {
                Size = new Size(400, 40),
                BackColor = Color.Transparent
            };

            var label = new Label
            {
                Text = "IO状态:",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(0, 10),
                AutoSize = true
            };
            panel.Controls.Add(label);

            _ioIndicators = new Panel[8];
            for (int i = 0; i < 8; i++)
            {
                _ioIndicators[i] = new Panel
                {
                    Size = new Size(20, 20),
                    Location = new Point(60 + i * 30, 8),
                    BackColor = Color.FromArgb(158, 158, 158)
                };
                panel.Controls.Add(_ioIndicators[i]);
            }

            return panel;
        }

        private void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        #endregion
    }
}
