using System;
using System.Drawing;
using System.Windows.Forms;

namespace GJVdc32Tool.Builders
{
    /// <summary>
    /// 负载通道面板构建器
    /// 职责：构建 GJDD-750 的 8 通道负载控制面板
    /// </summary>
    public class LoadChannelPanelBuilder
    {
        #region 常量定义

        private const int CHANNEL_COUNT = 8;
        private const int PANEL_WIDTH = 280;
        private const int PANEL_HEIGHT = 120;
        private const int PANEL_MARGIN = 10;
        private const int COLUMNS = 4;

        #endregion

        #region 私有字段

        private readonly Panel _container;
        private readonly Panel[] _channelPanels;
        private readonly Label[] _channelLabels;
        private readonly Label[] _currentLabels;
        private readonly Label[] _voltageLabels;
        private readonly Label[] _powerLabels;
        private readonly Button[] _toggleButtons;
        private readonly Panel[] _statusIndicators;

        private Color _onColor = Color.FromArgb(76, 175, 80);
        private Color _offColor = Color.FromArgb(158, 158, 158);
        private Color _faultColor = Color.FromArgb(244, 67, 54);
        private Font _labelFont;
        private Font _valueFont;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建负载通道面板构建器
        /// </summary>
        /// <param name="container">父容器面板</param>
        public LoadChannelPanelBuilder(Panel container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _channelPanels = new Panel[CHANNEL_COUNT];
            _channelLabels = new Label[CHANNEL_COUNT];
            _currentLabels = new Label[CHANNEL_COUNT];
            _voltageLabels = new Label[CHANNEL_COUNT];
            _powerLabels = new Label[CHANNEL_COUNT];
            _toggleButtons = new Button[CHANNEL_COUNT];
            _statusIndicators = new Panel[CHANNEL_COUNT];

            _labelFont = new Font("Segoe UI", 9f, FontStyle.Regular);
            _valueFont = new Font("Segoe UI", 11f, FontStyle.Bold);
        }

        #endregion

        #region 链式配置方法

        /// <summary>
        /// 设置开启状态颜色
        /// </summary>
        public LoadChannelPanelBuilder WithOnColor(Color color)
        {
            _onColor = color;
            return this;
        }

        /// <summary>
        /// 设置关闭状态颜色
        /// </summary>
        public LoadChannelPanelBuilder WithOffColor(Color color)
        {
            _offColor = color;
            return this;
        }

        /// <summary>
        /// 设置故障状态颜色
        /// </summary>
        public LoadChannelPanelBuilder WithFaultColor(Color color)
        {
            _faultColor = color;
            return this;
        }

        #endregion

        #region 构建方法

        /// <summary>
        /// 构建所有负载通道面板
        /// </summary>
        /// <returns>构建结果</returns>
        public LoadChannelPanelBuildResult Build()
        {
            _container.SuspendLayout();

            try
            {
                _container.Controls.Clear();

                for (int i = 0; i < CHANNEL_COUNT; i++)
                {
                    int row = i / COLUMNS;
                    int col = i % COLUMNS;

                    var channelPanel = CreateLoadChannelPanel(i, row, col);
                    _container.Controls.Add(channelPanel);
                    _channelPanels[i] = channelPanel;
                }

                return new LoadChannelPanelBuildResult
                {
                    Success = true,
                    ChannelPanels = _channelPanels,
                    ChannelLabels = _channelLabels,
                    CurrentLabels = _currentLabels,
                    VoltageLabels = _voltageLabels,
                    PowerLabels = _powerLabels,
                    ToggleButtons = _toggleButtons,
                    StatusIndicators = _statusIndicators,
                    TotalWidth = COLUMNS * (PANEL_WIDTH + PANEL_MARGIN),
                    TotalHeight = 2 * (PANEL_HEIGHT + PANEL_MARGIN)
                };
            }
            finally
            {
                _container.ResumeLayout(true);
            }
        }

        #endregion

        #region 私有方法

        private Panel CreateLoadChannelPanel(int channelIndex, int row, int col)
        {
            var panel = new Panel
            {
                Size = new Size(PANEL_WIDTH, PANEL_HEIGHT),
                Location = new Point(
                    col * (PANEL_WIDTH + PANEL_MARGIN),
                    row * (PANEL_HEIGHT + PANEL_MARGIN)
                ),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = channelIndex
            };

            // 标题行：状态指示器 + 通道名
            var indicator = CreateStatusIndicator(channelIndex);
            panel.Controls.Add(indicator);
            _statusIndicators[channelIndex] = indicator;

            var channelLabel = CreateChannelTitleLabel(channelIndex);
            panel.Controls.Add(channelLabel);
            _channelLabels[channelIndex] = channelLabel;

            // 数据行
            var dataPanel = CreateDataPanel(channelIndex);
            panel.Controls.Add(dataPanel);

            // 控制按钮
            var toggleBtn = CreateToggleButton(channelIndex);
            panel.Controls.Add(toggleBtn);
            _toggleButtons[channelIndex] = toggleBtn;

            return panel;
        }

        private Panel CreateStatusIndicator(int channelIndex)
        {
            return new Panel
            {
                Size = new Size(12, 12),
                Location = new Point(10, 10),
                BackColor = _offColor,
                Tag = channelIndex
            };
        }

        private Label CreateChannelTitleLabel(int channelIndex)
        {
            return new Label
            {
                Text = $"通道 {channelIndex + 1}",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(66, 66, 66),
                Location = new Point(28, 8),
                AutoSize = true,
                Tag = channelIndex
            };
        }

        private Panel CreateDataPanel(int channelIndex)
        {
            var dataPanel = new Panel
            {
                Location = new Point(10, 32),
                Size = new Size(180, 75),
                BackColor = Color.Transparent
            };

            // 电流
            var currentLbl = new Label
            {
                Text = "电流:",
                Font = _labelFont,
                Location = new Point(0, 0),
                AutoSize = true
            };
            dataPanel.Controls.Add(currentLbl);

            var currentValue = new Label
            {
                Text = "--.- A",
                Font = _valueFont,
                ForeColor = Color.FromArgb(33, 150, 243),
                Location = new Point(50, 0),
                AutoSize = true,
                Tag = channelIndex
            };
            dataPanel.Controls.Add(currentValue);
            _currentLabels[channelIndex] = currentValue;

            // 电压
            var voltageLbl = new Label
            {
                Text = "电压:",
                Font = _labelFont,
                Location = new Point(0, 24),
                AutoSize = true
            };
            dataPanel.Controls.Add(voltageLbl);

            var voltageValue = new Label
            {
                Text = "--.- V",
                Font = _valueFont,
                ForeColor = Color.FromArgb(76, 175, 80),
                Location = new Point(50, 24),
                AutoSize = true,
                Tag = channelIndex
            };
            dataPanel.Controls.Add(voltageValue);
            _voltageLabels[channelIndex] = voltageValue;

            // 功率
            var powerLbl = new Label
            {
                Text = "功率:",
                Font = _labelFont,
                Location = new Point(0, 48),
                AutoSize = true
            };
            dataPanel.Controls.Add(powerLbl);

            var powerValue = new Label
            {
                Text = "--.- W",
                Font = _valueFont,
                ForeColor = Color.FromArgb(255, 152, 0),
                Location = new Point(50, 48),
                AutoSize = true,
                Tag = channelIndex
            };
            dataPanel.Controls.Add(powerValue);
            _powerLabels[channelIndex] = powerValue;

            return dataPanel;
        }

        private Button CreateToggleButton(int channelIndex)
        {
            return new Button
            {
                Text = "开启",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Size = new Size(70, 32),
                Location = new Point(200, 70),
                BackColor = _offColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = channelIndex
            };
        }

        #endregion
    }

    /// <summary>
    /// 负载通道面板构建结果
    /// </summary>
    public class LoadChannelPanelBuildResult
    {
        public bool Success { get; set; }
        public Panel[] ChannelPanels { get; set; }
        public Label[] ChannelLabels { get; set; }
        public Label[] CurrentLabels { get; set; }
        public Label[] VoltageLabels { get; set; }
        public Label[] PowerLabels { get; set; }
        public Button[] ToggleButtons { get; set; }
        public Panel[] StatusIndicators { get; set; }
        public int TotalWidth { get; set; }
        public int TotalHeight { get; set; }
    }
}
