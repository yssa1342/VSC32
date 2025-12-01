using System;
using System.Drawing;
using System.Windows.Forms;

namespace GJVdc32Tool.Builders
{
    /// <summary>
    /// 通道面板构建器
    /// 职责：构建 VDC-32 的 32 通道显示面板
    /// </summary>
    /// <remarks>
    /// 遵循 Builder 模式，支持链式调用
    /// </remarks>
    public class ChannelPanelBuilder
    {
        #region 常量定义

        private const int CHANNEL_COUNT = 32;
        private const int COLUMNS = 8;
        private const int ROWS = 4;
        private const int PANEL_WIDTH = 120;
        private const int PANEL_HEIGHT = 80;
        private const int PANEL_MARGIN = 5;
        private const int INDICATOR_SIZE = 12;

        #endregion

        #region 私有字段

        private readonly Panel _container;
        private readonly Label[] _voltageLabels;
        private readonly Label[] _channelLabels;
        private readonly Panel[] _indicatorPanels;

        private Color _normalColor = Color.FromArgb(76, 175, 80);
        private Color _alarmColor = Color.FromArgb(244, 67, 54);
        private Color _offlineColor = Color.FromArgb(158, 158, 158);
        private Color _panelBackColor = Color.White;
        private Font _voltageFont;
        private Font _channelFont;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建通道面板构建器
        /// </summary>
        /// <param name="container">父容器面板</param>
        public ChannelPanelBuilder(Panel container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _voltageLabels = new Label[CHANNEL_COUNT];
            _channelLabels = new Label[CHANNEL_COUNT];
            _indicatorPanels = new Panel[CHANNEL_COUNT];

            _voltageFont = new Font("Segoe UI", 14f, FontStyle.Bold);
            _channelFont = new Font("Segoe UI", 9f, FontStyle.Regular);
        }

        #endregion

        #region 链式配置方法

        /// <summary>
        /// 设置正常状态颜色
        /// </summary>
        public ChannelPanelBuilder WithNormalColor(Color color)
        {
            _normalColor = color;
            return this;
        }

        /// <summary>
        /// 设置报警状态颜色
        /// </summary>
        public ChannelPanelBuilder WithAlarmColor(Color color)
        {
            _alarmColor = color;
            return this;
        }

        /// <summary>
        /// 设置离线状态颜色
        /// </summary>
        public ChannelPanelBuilder WithOfflineColor(Color color)
        {
            _offlineColor = color;
            return this;
        }

        /// <summary>
        /// 设置面板背景色
        /// </summary>
        public ChannelPanelBuilder WithPanelBackColor(Color color)
        {
            _panelBackColor = color;
            return this;
        }

        /// <summary>
        /// 设置电压字体
        /// </summary>
        public ChannelPanelBuilder WithVoltageFont(Font font)
        {
            _voltageFont = font ?? throw new ArgumentNullException(nameof(font));
            return this;
        }

        /// <summary>
        /// 设置通道标签字体
        /// </summary>
        public ChannelPanelBuilder WithChannelFont(Font font)
        {
            _channelFont = font ?? throw new ArgumentNullException(nameof(font));
            return this;
        }

        #endregion

        #region 构建方法

        /// <summary>
        /// 构建所有通道面板
        /// </summary>
        /// <returns>构建结果</returns>
        public ChannelPanelBuildResult Build()
        {
            _container.SuspendLayout();

            try
            {
                _container.Controls.Clear();

                for (int i = 0; i < CHANNEL_COUNT; i++)
                {
                    int row = i / COLUMNS;
                    int col = i % COLUMNS;

                    var channelPanel = CreateChannelPanel(i, row, col);
                    _container.Controls.Add(channelPanel);
                }

                return new ChannelPanelBuildResult
                {
                    Success = true,
                    VoltageLabels = _voltageLabels,
                    ChannelLabels = _channelLabels,
                    IndicatorPanels = _indicatorPanels,
                    TotalWidth = COLUMNS * (PANEL_WIDTH + PANEL_MARGIN),
                    TotalHeight = ROWS * (PANEL_HEIGHT + PANEL_MARGIN)
                };
            }
            finally
            {
                _container.ResumeLayout(true);
            }
        }

        #endregion

        #region 私有方法

        private Panel CreateChannelPanel(int channelIndex, int row, int col)
        {
            var panel = new Panel
            {
                Size = new Size(PANEL_WIDTH, PANEL_HEIGHT),
                Location = new Point(
                    col * (PANEL_WIDTH + PANEL_MARGIN),
                    row * (PANEL_HEIGHT + PANEL_MARGIN)
                ),
                BackColor = _panelBackColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            // 状态指示器
            var indicator = CreateIndicator(channelIndex);
            panel.Controls.Add(indicator);
            _indicatorPanels[channelIndex] = indicator;

            // 通道标签
            var channelLabel = CreateChannelLabel(channelIndex);
            panel.Controls.Add(channelLabel);
            _channelLabels[channelIndex] = channelLabel;

            // 电压显示
            var voltageLabel = CreateVoltageLabel(channelIndex);
            panel.Controls.Add(voltageLabel);
            _voltageLabels[channelIndex] = voltageLabel;

            return panel;
        }

        private Panel CreateIndicator(int channelIndex)
        {
            return new Panel
            {
                Size = new Size(INDICATOR_SIZE, INDICATOR_SIZE),
                Location = new Point(8, 8),
                BackColor = _offlineColor,
                Tag = channelIndex
            };
        }

        private Label CreateChannelLabel(int channelIndex)
        {
            return new Label
            {
                Text = $"CH{channelIndex + 1:D2}",
                Font = _channelFont,
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(24, 6),
                AutoSize = true,
                Tag = channelIndex
            };
        }

        private Label CreateVoltageLabel(int channelIndex)
        {
            return new Label
            {
                Text = "--.- V",
                Font = _voltageFont,
                ForeColor = Color.FromArgb(66, 66, 66),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 40,
                Tag = channelIndex
            };
        }

        #endregion
    }

    /// <summary>
    /// 通道面板构建结果
    /// </summary>
    public class ChannelPanelBuildResult
    {
        /// <summary>
        /// 是否构建成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 电压显示标签数组
        /// </summary>
        public Label[] VoltageLabels { get; set; }

        /// <summary>
        /// 通道名称标签数组
        /// </summary>
        public Label[] ChannelLabels { get; set; }

        /// <summary>
        /// 状态指示器面板数组
        /// </summary>
        public Panel[] IndicatorPanels { get; set; }

        /// <summary>
        /// 总宽度
        /// </summary>
        public int TotalWidth { get; set; }

        /// <summary>
        /// 总高度
        /// </summary>
        public int TotalHeight { get; set; }
    }
}
