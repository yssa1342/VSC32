using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GJVdc32Tool.Views
{
    /// <summary>
    /// 日志视图控件
    /// 职责：显示运行日志
    /// </summary>
    public partial class LogView : UserControl
    {
        #region 常量定义

        private const int MAX_LOG_ENTRIES = 1000;

        private static readonly Color COLOR_SUCCESS = Color.FromArgb(76, 175, 80);
        private static readonly Color COLOR_ERROR = Color.FromArgb(244, 67, 54);
        private static readonly Color COLOR_INFO = Color.FromArgb(66, 66, 66);

        #endregion

        #region 私有字段

        private RichTextBox _txtLog;
        private Button _btnClear;
        private Button _btnExport;
        private CheckBox _chkAutoScroll;
        private Label _lblCount;

        private readonly List<LogEntry> _logEntries;
        private readonly object _lockObject = new object();

        #endregion

        #region 事件

        public event EventHandler ClearRequested;
        public event EventHandler ExportRequested;

        #endregion

        #region 属性

        public bool AutoScroll
        {
            get => _chkAutoScroll?.Checked ?? true;
            set { if (_chkAutoScroll != null) _chkAutoScroll.Checked = value; }
        }

        public int LogCount
        {
            get
            {
                lock (_lockObject)
                {
                    return _logEntries.Count;
                }
            }
        }

        #endregion

        #region 构造函数

        public LogView()
        {
            _logEntries = new List<LogEntry>();
            InitializeComponent();
            BuildUI();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 添加日志
        /// </summary>
        public void AddLog(string message, bool? success = null)
        {
            var entry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = message,
                Success = success
            };

            lock (_lockObject)
            {
                _logEntries.Add(entry);

                // 限制日志数量
                if (_logEntries.Count > MAX_LOG_ENTRIES)
                {
                    _logEntries.RemoveAt(0);
                }
            }

            InvokeIfRequired(() => AppendLogToTextBox(entry));
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        public void ClearLogs()
        {
            lock (_lockObject)
            {
                _logEntries.Clear();
            }

            InvokeIfRequired(() =>
            {
                _txtLog.Clear();
                UpdateLogCount();
            });
        }

        /// <summary>
        /// 获取所有日志文本
        /// </summary>
        public string GetAllLogsText()
        {
            lock (_lockObject)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var entry in _logEntries)
                {
                    string prefix = GetLogPrefix(entry.Success);
                    sb.AppendLine($"[{entry.Timestamp:HH:mm:ss}] {prefix} {entry.Message}");
                }
                return sb.ToString();
            }
        }

        #endregion

        #region 私有方法

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Name = "LogView";
            this.Size = new Size(800, 400);
            this.ResumeLayout(false);
        }

        private void BuildUI()
        {
            this.SuspendLayout();

            // 工具栏
            var toolPanel = CreateToolPanel();
            toolPanel.Dock = DockStyle.Top;
            this.Controls.Add(toolPanel);

            // 日志文本框
            _txtLog = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9f),
                BackColor = Color.White,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };
            this.Controls.Add(_txtLog);

            // 确保 TextBox 在下面
            _txtLog.BringToFront();

            this.ResumeLayout(true);
        }

        private Panel CreateToolPanel()
        {
            var panel = new Panel
            {
                Height = 40,
                BackColor = Color.White,
                Padding = new Padding(5)
            };

            var titleLabel = new Label
            {
                Text = "● 运行日志",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                Location = new Point(10, 10),
                AutoSize = true
            };
            panel.Controls.Add(titleLabel);

            _chkAutoScroll = new CheckBox
            {
                Text = "自动滚动",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(120, 10),
                AutoSize = true,
                Checked = true
            };
            panel.Controls.Add(_chkAutoScroll);

            _lblCount = new Label
            {
                Text = "共 0 条",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(220, 12),
                AutoSize = true
            };
            panel.Controls.Add(_lblCount);

            _btnClear = new Button
            {
                Text = "清空",
                Font = new Font("Segoe UI", 9f),
                Size = new Size(60, 26),
                Location = new Point(600, 7),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnClear.Click += (s, e) =>
            {
                ClearLogs();
                ClearRequested?.Invoke(this, EventArgs.Empty);
            };
            panel.Controls.Add(_btnClear);

            _btnExport = new Button
            {
                Text = "导出",
                Font = new Font("Segoe UI", 9f),
                Size = new Size(60, 26),
                Location = new Point(670, 7),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _btnExport.Click += (s, e) => ExportRequested?.Invoke(this, EventArgs.Empty);
            panel.Controls.Add(_btnExport);

            return panel;
        }

        private void AppendLogToTextBox(LogEntry entry)
        {
            string prefix = GetLogPrefix(entry.Success);
            Color color = GetLogColor(entry.Success);

            string line = $"[{entry.Timestamp:HH:mm:ss}] {prefix} {entry.Message}\n";

            _txtLog.SelectionStart = _txtLog.TextLength;
            _txtLog.SelectionLength = 0;
            _txtLog.SelectionColor = color;
            _txtLog.AppendText(line);

            if (AutoScroll)
            {
                _txtLog.ScrollToCaret();
            }

            UpdateLogCount();
        }

        private string GetLogPrefix(bool? success)
        {
            if (success == true) return "✓";
            if (success == false) return "✗";
            return "ℹ";
        }

        private Color GetLogColor(bool? success)
        {
            if (success == true) return COLOR_SUCCESS;
            if (success == false) return COLOR_ERROR;
            return COLOR_INFO;
        }

        private void UpdateLogCount()
        {
            _lblCount.Text = $"共 {LogCount} 条";
        }

        private void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        #endregion

        #region 内部类

        private class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }
            public bool? Success { get; set; }
        }

        #endregion
    }
}
