using System;
using System.Drawing;
using System.Windows.Forms;
using GJVdc32Tool.Builders;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Views
{
    /// <summary>
    /// GJDD-750 负载设备控件
    /// 职责：显示 8 通道负载数据和控制
    /// </summary>
    public partial class LoadDeviceView : UserControl, ILoadDeviceView
    {
        #region 常量定义

        private const int CHANNEL_COUNT = 8;

        #endregion

        #region 私有字段

        private Panel[] _channelPanels;
        private Label[] _currentLabels;
        private Label[] _voltageLabels;
        private Label[] _powerLabels;
        private Button[] _toggleButtons;
        private Panel[] _statusIndicators;

        private Label _lblInverterVoltage;
        private Label _lblInverterCurrent;
        private Label _lblInverterPower;
        private Label _lblInverterStatus;

        private NumericUpDown _numSingleCurrent;
        private ComboBox _cmbSingleChannel;
        private NumericUpDown _numBatchCurrent;

        private bool _singleChannelConfigEnabled = true;
        private bool _batchConfigEnabled = true;

        #endregion

        #region 事件

        public event EventHandler<int> ChannelToggleRequested;
        public event EventHandler<ChannelCurrentEventArgs> CurrentSetRequested;
        public event EventHandler BatchTurnOnRequested;
        public event EventHandler BatchTurnOffRequested;
        public event EventHandler<double> BatchCurrentSetRequested;

        #endregion

        #region 属性

        public string InverterVoltage
        {
            get => _lblInverterVoltage?.Text ?? string.Empty;
            set { if (_lblInverterVoltage != null) _lblInverterVoltage.Text = value; }
        }

        public string InverterCurrent
        {
            get => _lblInverterCurrent?.Text ?? string.Empty;
            set { if (_lblInverterCurrent != null) _lblInverterCurrent.Text = value; }
        }

        public string InverterPower
        {
            get => _lblInverterPower?.Text ?? string.Empty;
            set { if (_lblInverterPower != null) _lblInverterPower.Text = value; }
        }

        public string InverterStatus
        {
            get => _lblInverterStatus?.Text ?? string.Empty;
            set { if (_lblInverterStatus != null) _lblInverterStatus.Text = value; }
        }

        public bool SingleChannelConfigEnabled
        {
            get => _singleChannelConfigEnabled;
            set
            {
                _singleChannelConfigEnabled = value;
                UpdateConfigPanelState();
            }
        }

        public bool BatchConfigEnabled
        {
            get => _batchConfigEnabled;
            set
            {
                _batchConfigEnabled = value;
                UpdateConfigPanelState();
            }
        }

        /// <summary>
        /// 电流标签数组（供 Handler 使用）
        /// </summary>
        public Label[] CurrentLabels => _currentLabels;

        /// <summary>
        /// 功率标签数组
        /// </summary>
        public Label[] PowerLabels => _powerLabels;

        /// <summary>
        /// 状态指示器数组
        /// </summary>
        public Panel[] StatusIndicators => _statusIndicators;

        /// <summary>
        /// 开关按钮数组
        /// </summary>
        public Button[] ToggleButtons => _toggleButtons;

        #endregion

        #region 构造函数

        public LoadDeviceView()
        {
            InitializeComponent();
            BuildUI();
        }

        #endregion

        #region ILoadDeviceView 实现

        public void UpdateChannelData(int channelIndex, double current, double power, bool isOn)
        {
            if (channelIndex < 0 || channelIndex >= CHANNEL_COUNT)
                return;

            InvokeIfRequired(() =>
            {
                _currentLabels[channelIndex].Text = $"{current:F2} A";
                _powerLabels[channelIndex].Text = $"{power:F1} W";

                _statusIndicators[channelIndex].BackColor = isOn
                    ? Color.FromArgb(76, 175, 80)
                    : Color.FromArgb(158, 158, 158);

                _toggleButtons[channelIndex].Text = isOn ? "关闭" : "开启";
                _toggleButtons[channelIndex].BackColor = isOn
                    ? Color.FromArgb(76, 175, 80)
                    : Color.FromArgb(158, 158, 158);
            });
        }

        public void ShowChannelError(int channelIndex, string message)
        {
            if (channelIndex < 0 || channelIndex >= CHANNEL_COUNT)
                return;

            InvokeIfRequired(() =>
            {
                _statusIndicators[channelIndex].BackColor = Color.FromArgb(244, 67, 54);
                MessageBox.Show(
                    $"通道 {channelIndex + 1} 操作失败:\n{message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            });
        }

        public void ResetAllChannels()
        {
            InvokeIfRequired(() =>
            {
                for (int i = 0; i < CHANNEL_COUNT; i++)
                {
                    _currentLabels[i].Text = "--.- A";
                    _powerLabels[i].Text = "--.- W";
                    _statusIndicators[i].BackColor = Color.FromArgb(158, 158, 158);
                    _toggleButtons[i].Text = "开启";
                    _toggleButtons[i].BackColor = Color.FromArgb(158, 158, 158);
                }

                InverterVoltage = "--.- V";
                InverterCurrent = "--.- A";
                InverterPower = "--.- W";
                InverterStatus = "离线";
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
            this.Name = "LoadDeviceView";
            this.Size = new Size(1000, 550);
            this.ResumeLayout(false);
        }

        private void BuildUI()
        {
            this.SuspendLayout();

            // 变频器状态区域
            var inverterPanel = CreateInverterPanel();
            inverterPanel.Location = new Point(10, 10);
            this.Controls.Add(inverterPanel);

            // 通道显示区域
            var channelContainer = new Panel
            {
                Location = new Point(10, 80),
                Size = new Size(980, 280),
                AutoScroll = true
            };
            this.Controls.Add(channelContainer);

            // 使用 Builder 构建通道面板
            var builder = new LoadChannelPanelBuilder(channelContainer);
            var result = builder.Build();

            if (result.Success)
            {
                _channelPanels = result.ChannelPanels;
                _currentLabels = result.CurrentLabels;
                _powerLabels = result.PowerLabels;
                _toggleButtons = result.ToggleButtons;
                _statusIndicators = result.StatusIndicators;
                _voltageLabels = result.VoltageLabels;

                // 绑定按钮事件
                BindToggleButtonEvents();
            }

            // 配置区域
            var configPanel = CreateConfigPanel();
            configPanel.Location = new Point(10, 370);
            this.Controls.Add(configPanel);

            this.ResumeLayout(true);
        }

        private Panel CreateInverterPanel()
        {
            var panel = new Panel
            {
                Size = new Size(980, 60),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var titleLabel = new Label
            {
                Text = "● 变频器状态",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 152, 0),
                Location = new Point(10, 5),
                AutoSize = true
            };
            panel.Controls.Add(titleLabel);

            // 电压
            panel.Controls.Add(CreateStaticLabel("电压:", 10, 30));
            _lblInverterVoltage = CreateValueLabel("--.- V", 50, 30, Color.FromArgb(33, 150, 243));
            panel.Controls.Add(_lblInverterVoltage);

            // 电流
            panel.Controls.Add(CreateStaticLabel("电流:", 150, 30));
            _lblInverterCurrent = CreateValueLabel("--.- A", 190, 30, Color.FromArgb(76, 175, 80));
            panel.Controls.Add(_lblInverterCurrent);

            // 功率
            panel.Controls.Add(CreateStaticLabel("功率:", 290, 30));
            _lblInverterPower = CreateValueLabel("--.- W", 330, 30, Color.FromArgb(255, 152, 0));
            panel.Controls.Add(_lblInverterPower);

            // 状态
            panel.Controls.Add(CreateStaticLabel("状态:", 430, 30));
            _lblInverterStatus = CreateValueLabel("离线", 470, 30, Color.FromArgb(158, 158, 158));
            panel.Controls.Add(_lblInverterStatus);

            return panel;
        }

        private Panel CreateConfigPanel()
        {
            var panel = new Panel
            {
                Size = new Size(980, 160),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // 单通道配置
            var singleTitle = new Label
            {
                Text = "● 单通道配置",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                Location = new Point(10, 5),
                AutoSize = true
            };
            panel.Controls.Add(singleTitle);

            panel.Controls.Add(CreateStaticLabel("通道:", 10, 35));
            _cmbSingleChannel = new ComboBox
            {
                Location = new Point(50, 32),
                Size = new Size(60, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 1; i <= CHANNEL_COUNT; i++)
                _cmbSingleChannel.Items.Add(i);
            _cmbSingleChannel.SelectedIndex = 0;
            panel.Controls.Add(_cmbSingleChannel);

            panel.Controls.Add(CreateStaticLabel("电流(A):", 130, 35));
            _numSingleCurrent = new NumericUpDown
            {
                Location = new Point(185, 32),
                Size = new Size(70, 25),
                Minimum = 0,
                Maximum = 60,
                DecimalPlaces = 1,
                Value = 10
            };
            panel.Controls.Add(_numSingleCurrent);

            var btnSetSingle = CreateActionButton("设置", 270, 30);
            btnSetSingle.Click += OnSetSingleCurrentClick;
            panel.Controls.Add(btnSetSingle);

            // 批量配置
            var batchTitle = new Label
            {
                Text = "● 批量配置",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 175, 80),
                Location = new Point(10, 70),
                AutoSize = true
            };
            panel.Controls.Add(batchTitle);

            panel.Controls.Add(CreateStaticLabel("电流(A):", 10, 100));
            _numBatchCurrent = new NumericUpDown
            {
                Location = new Point(65, 97),
                Size = new Size(70, 25),
                Minimum = 0,
                Maximum = 60,
                DecimalPlaces = 1,
                Value = 10
            };
            panel.Controls.Add(_numBatchCurrent);

            var btnSetAll = CreateActionButton("全部设置", 150, 95);
            btnSetAll.Click += OnSetAllCurrentClick;
            panel.Controls.Add(btnSetAll);

            var btnTurnOnAll = CreateActionButton("全部开启", 240, 95, Color.FromArgb(76, 175, 80));
            btnTurnOnAll.Click += (s, e) => BatchTurnOnRequested?.Invoke(this, EventArgs.Empty);
            panel.Controls.Add(btnTurnOnAll);

            var btnTurnOffAll = CreateActionButton("全部关闭", 330, 95, Color.FromArgb(244, 67, 54));
            btnTurnOffAll.Click += (s, e) => BatchTurnOffRequested?.Invoke(this, EventArgs.Empty);
            panel.Controls.Add(btnTurnOffAll);

            return panel;
        }

        private Label CreateStaticLabel(string text, int x, int y)
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

        private Label CreateValueLabel(string text, int x, int y, Color foreColor)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = foreColor,
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private Button CreateActionButton(string text, int x, int y, Color? backColor = null)
        {
            return new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9f),
                Size = new Size(80, 28),
                Location = new Point(x, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor ?? Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
        }

        private void BindToggleButtonEvents()
        {
            for (int i = 0; i < CHANNEL_COUNT; i++)
            {
                int channelIndex = i;
                _toggleButtons[i].Click += (s, e) =>
                {
                    ChannelToggleRequested?.Invoke(this, channelIndex);
                };
            }
        }

        private void OnSetSingleCurrentClick(object sender, EventArgs e)
        {
            int channelIndex = _cmbSingleChannel.SelectedIndex;
            double current = (double)_numSingleCurrent.Value;
            CurrentSetRequested?.Invoke(this, new ChannelCurrentEventArgs(channelIndex, current));
        }

        private void OnSetAllCurrentClick(object sender, EventArgs e)
        {
            double current = (double)_numBatchCurrent.Value;
            BatchCurrentSetRequested?.Invoke(this, current);
        }

        private void UpdateConfigPanelState()
        {
            InvokeIfRequired(() =>
            {
                if (_cmbSingleChannel != null)
                    _cmbSingleChannel.Enabled = _singleChannelConfigEnabled;
                if (_numSingleCurrent != null)
                    _numSingleCurrent.Enabled = _singleChannelConfigEnabled;
                if (_numBatchCurrent != null)
                    _numBatchCurrent.Enabled = _batchConfigEnabled;
            });
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
