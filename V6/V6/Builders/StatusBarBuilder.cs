using System;
using System.Drawing;
using System.Windows.Forms;

namespace GJVdc32Tool.Builders
{
    /// <summary>
    /// 状态栏构建器
    /// 职责：构建底部状态栏 UI
    /// </summary>
    public class StatusBarBuilder
    {
        #region 私有字段

        private readonly Panel _container;
        private Color _backColor = Color.FromArgb(240, 240, 240);
        private Color _textColor = Color.FromArgb(66, 66, 66);
        private Color _successColor = Color.FromArgb(76, 175, 80);
        private Color _errorColor = Color.FromArgb(244, 67, 54);
        private Font _font;

        private Label _statusLabel;
        private Label _timeLabel;
        private Label _connectionLabel;
        private Label _pollingLabel;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建状态栏构建器
        /// </summary>
        /// <param name="container">状态栏容器面板</param>
        public StatusBarBuilder(Panel container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _font = new Font("Segoe UI", 9f, FontStyle.Regular);
        }

        #endregion

        #region 链式配置方法

        /// <summary>
        /// 设置背景色
        /// </summary>
        public StatusBarBuilder WithBackColor(Color color)
        {
            _backColor = color;
            return this;
        }

        /// <summary>
        /// 设置文本颜色
        /// </summary>
        public StatusBarBuilder WithTextColor(Color color)
        {
            _textColor = color;
            return this;
        }

        #endregion

        #region 构建方法

        /// <summary>
        /// 构建状态栏
        /// </summary>
        /// <returns>构建结果</returns>
        public StatusBarBuildResult Build()
        {
            _container.SuspendLayout();

            try
            {
                _container.Controls.Clear();
                _container.BackColor = _backColor;
                _container.Height = 28;

                // 状态信息（左侧）
                _statusLabel = new Label
                {
                    Text = "就绪",
                    Font = _font,
                    ForeColor = _textColor,
                    Location = new Point(10, 5),
                    AutoSize = true
                };
                _container.Controls.Add(_statusLabel);

                // 轮询状态
                _pollingLabel = new Label
                {
                    Text = "轮询: 停止",
                    Font = _font,
                    ForeColor = _textColor,
                    Location = new Point(200, 5),
                    AutoSize = true
                };
                _container.Controls.Add(_pollingLabel);

                // 连接状态
                _connectionLabel = new Label
                {
                    Text = "● 未连接",
                    Font = _font,
                    ForeColor = _errorColor,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    AutoSize = true
                };
                _connectionLabel.Location = new Point(_container.Width - 200, 5);
                _container.Controls.Add(_connectionLabel);

                // 时间（右侧）
                _timeLabel = new Label
                {
                    Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Font = _font,
                    ForeColor = _textColor,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    AutoSize = true
                };
                _timeLabel.Location = new Point(_container.Width - 100, 5);
                _container.Controls.Add(_timeLabel);

                return new StatusBarBuildResult
                {
                    Success = true,
                    StatusLabel = _statusLabel,
                    TimeLabel = _timeLabel,
                    ConnectionLabel = _connectionLabel,
                    PollingLabel = _pollingLabel
                };
            }
            finally
            {
                _container.ResumeLayout(true);
            }
        }

        #endregion
    }

    /// <summary>
    /// 状态栏构建结果
    /// </summary>
    public class StatusBarBuildResult
    {
        public bool Success { get; set; }
        public Label StatusLabel { get; set; }
        public Label TimeLabel { get; set; }
        public Label ConnectionLabel { get; set; }
        public Label PollingLabel { get; set; }
    }
}
