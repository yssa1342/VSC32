using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace GJVdc32Tool.Builders
{
    /// <summary>
    /// 连接配置面板构建器
    /// 职责：构建串口/TCP 连接配置 UI 组件
    /// </summary>
    public class ConnectionPanelBuilder
    {
        #region 常量定义

        private static readonly int[] BAUD_RATES = { 9600, 19200, 38400, 57600, 115200 };
        private const int DEFAULT_BAUD_RATE = 9600;
        private const int DEFAULT_TCP_PORT = 502;
        private const byte DEFAULT_SLAVE_ID = 1;

        #endregion

        #region 私有字段

        private readonly Panel _container;
        private string _title = "连接配置";
        private bool _showTcpOption = true;
        private bool _showSlaveId = true;
        private Color _accentColor = Color.FromArgb(33, 150, 243);

        // 构建结果控件引用
        private RadioButton _serialRadio;
        private RadioButton _tcpRadio;
        private ComboBox _portCombo;
        private ComboBox _baudCombo;
        private TextBox _ipTextBox;
        private NumericUpDown _tcpPortNumeric;
        private NumericUpDown _slaveIdNumeric;
        private Button _connectButton;
        private Button _refreshButton;
        private Label _statusLabel;
        private Panel _serialPanel;
        private Panel _tcpPanel;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建连接配置面板构建器
        /// </summary>
        /// <param name="container">父容器面板</param>
        public ConnectionPanelBuilder(Panel container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        #endregion

        #region 链式配置方法

        /// <summary>
        /// 设置面板标题
        /// </summary>
        public ConnectionPanelBuilder WithTitle(string title)
        {
            _title = title ?? "连接配置";
            return this;
        }

        /// <summary>
        /// 是否显示 TCP 选项
        /// </summary>
        public ConnectionPanelBuilder WithTcpOption(bool show)
        {
            _showTcpOption = show;
            return this;
        }

        /// <summary>
        /// 是否显示从机地址
        /// </summary>
        public ConnectionPanelBuilder WithSlaveIdOption(bool show)
        {
            _showSlaveId = show;
            return this;
        }

        /// <summary>
        /// 设置主题色
        /// </summary>
        public ConnectionPanelBuilder WithAccentColor(Color color)
        {
            _accentColor = color;
            return this;
        }

        #endregion

        #region 构建方法

        /// <summary>
        /// 构建连接配置面板
        /// </summary>
        /// <returns>构建结果</returns>
        public ConnectionPanelBuildResult Build()
        {
            _container.SuspendLayout();

            try
            {
                _container.Controls.Clear();

                int yPos = 10;

                // 标题
                var titleLabel = CreateTitleLabel();
                titleLabel.Location = new Point(10, yPos);
                _container.Controls.Add(titleLabel);
                yPos += 30;

                // 连接类型选择（如果启用 TCP）
                if (_showTcpOption)
                {
                    var typePanel = CreateConnectionTypePanel();
                    typePanel.Location = new Point(10, yPos);
                    _container.Controls.Add(typePanel);
                    yPos += 35;
                }

                // 串口配置区域
                _serialPanel = CreateSerialConfigPanel();
                _serialPanel.Location = new Point(10, yPos);
                _container.Controls.Add(_serialPanel);

                // TCP 配置区域
                if (_showTcpOption)
                {
                    _tcpPanel = CreateTcpConfigPanel();
                    _tcpPanel.Location = new Point(10, yPos);
                    _tcpPanel.Visible = false;
                    _container.Controls.Add(_tcpPanel);
                }

                yPos += 70;

                // 从机地址（如果启用）
                if (_showSlaveId)
                {
                    var slavePanel = CreateSlaveIdPanel();
                    slavePanel.Location = new Point(10, yPos);
                    _container.Controls.Add(slavePanel);
                    yPos += 35;
                }

                // 连接按钮和状态
                var actionPanel = CreateActionPanel();
                actionPanel.Location = new Point(10, yPos);
                _container.Controls.Add(actionPanel);

                // 绑定事件
                BindEvents();

                return new ConnectionPanelBuildResult
                {
                    Success = true,
                    SerialRadio = _serialRadio,
                    TcpRadio = _tcpRadio,
                    PortComboBox = _portCombo,
                    BaudRateComboBox = _baudCombo,
                    IpTextBox = _ipTextBox,
                    TcpPortNumeric = _tcpPortNumeric,
                    SlaveIdNumeric = _slaveIdNumeric,
                    ConnectButton = _connectButton,
                    RefreshButton = _refreshButton,
                    StatusLabel = _statusLabel,
                    SerialPanel = _serialPanel,
                    TcpPanel = _tcpPanel
                };
            }
            finally
            {
                _container.ResumeLayout(true);
            }
        }

        #endregion

        #region 私有方法 - 创建控件

        private Label CreateTitleLabel()
        {
            return new Label
            {
                Text = $"● {_title}",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = _accentColor,
                AutoSize = true
            };
        }

        private Panel CreateConnectionTypePanel()
        {
            var panel = new Panel
            {
                Size = new Size(300, 30),
                BackColor = Color.Transparent
            };

            _serialRadio = new RadioButton
            {
                Text = "串口",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(0, 5),
                AutoSize = true,
                Checked = true
            };
            panel.Controls.Add(_serialRadio);

            _tcpRadio = new RadioButton
            {
                Text = "TCP/IP",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(80, 5),
                AutoSize = true
            };
            panel.Controls.Add(_tcpRadio);

            return panel;
        }

        private Panel CreateSerialConfigPanel()
        {
            var panel = new Panel
            {
                Size = new Size(350, 65),
                BackColor = Color.Transparent
            };

            // 串口选择
            var portLabel = new Label
            {
                Text = "串口:",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(0, 5),
                AutoSize = true
            };
            panel.Controls.Add(portLabel);

            _portCombo = new ComboBox
            {
                Font = new Font("Segoe UI", 9f),
                Location = new Point(50, 2),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            RefreshSerialPorts();
            panel.Controls.Add(_portCombo);

            _refreshButton = new Button
            {
                Text = "↻",
                Font = new Font("Segoe UI", 10f),
                Location = new Point(155, 1),
                Size = new Size(28, 25),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            panel.Controls.Add(_refreshButton);

            // 波特率
            var baudLabel = new Label
            {
                Text = "波特率:",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(0, 35),
                AutoSize = true
            };
            panel.Controls.Add(baudLabel);

            _baudCombo = new ComboBox
            {
                Font = new Font("Segoe UI", 9f),
                Location = new Point(50, 32),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var baud in BAUD_RATES)
            {
                _baudCombo.Items.Add(baud);
            }
            _baudCombo.SelectedItem = DEFAULT_BAUD_RATE;
            panel.Controls.Add(_baudCombo);

            return panel;
        }

        private Panel CreateTcpConfigPanel()
        {
            var panel = new Panel
            {
                Size = new Size(350, 65),
                BackColor = Color.Transparent
            };

            // IP 地址
            var ipLabel = new Label
            {
                Text = "IP:",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(0, 5),
                AutoSize = true
            };
            panel.Controls.Add(ipLabel);

            _ipTextBox = new TextBox
            {
                Font = new Font("Segoe UI", 9f),
                Location = new Point(50, 2),
                Size = new Size(150, 25),
                Text = "192.168.1.100"
            };
            panel.Controls.Add(_ipTextBox);

            // 端口
            var portLabel = new Label
            {
                Text = "端口:",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(0, 35),
                AutoSize = true
            };
            panel.Controls.Add(portLabel);

            _tcpPortNumeric = new NumericUpDown
            {
                Font = new Font("Segoe UI", 9f),
                Location = new Point(50, 32),
                Size = new Size(80, 25),
                Minimum = 1,
                Maximum = 65535,
                Value = DEFAULT_TCP_PORT
            };
            panel.Controls.Add(_tcpPortNumeric);

            return panel;
        }

        private Panel CreateSlaveIdPanel()
        {
            var panel = new Panel
            {
                Size = new Size(200, 30),
                BackColor = Color.Transparent
            };

            var label = new Label
            {
                Text = "从机地址:",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(0, 5),
                AutoSize = true
            };
            panel.Controls.Add(label);

            _slaveIdNumeric = new NumericUpDown
            {
                Font = new Font("Segoe UI", 9f),
                Location = new Point(70, 2),
                Size = new Size(60, 25),
                Minimum = 1,
                Maximum = 247,
                Value = DEFAULT_SLAVE_ID
            };
            panel.Controls.Add(_slaveIdNumeric);

            return panel;
        }

        private Panel CreateActionPanel()
        {
            var panel = new Panel
            {
                Size = new Size(350, 40),
                BackColor = Color.Transparent
            };

            _connectButton = new Button
            {
                Text = "连接",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Size = new Size(80, 32),
                Location = new Point(0, 0),
                BackColor = _accentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            panel.Controls.Add(_connectButton);

            _statusLabel = new Label
            {
                Text = "● 未连接",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(158, 158, 158),
                Location = new Point(90, 8),
                AutoSize = true
            };
            panel.Controls.Add(_statusLabel);

            return panel;
        }

        private void RefreshSerialPorts()
        {
            _portCombo.Items.Clear();
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                _portCombo.Items.Add(port);
            }
            if (_portCombo.Items.Count > 0)
            {
                _portCombo.SelectedIndex = 0;
            }
        }

        private void BindEvents()
        {
            if (_refreshButton != null)
            {
                _refreshButton.Click += (s, e) => RefreshSerialPorts();
            }

            if (_serialRadio != null && _tcpRadio != null)
            {
                _serialRadio.CheckedChanged += (s, e) =>
                {
                    if (_serialRadio.Checked)
                    {
                        _serialPanel.Visible = true;
                        if (_tcpPanel != null) _tcpPanel.Visible = false;
                    }
                };

                _tcpRadio.CheckedChanged += (s, e) =>
                {
                    if (_tcpRadio.Checked)
                    {
                        _serialPanel.Visible = false;
                        if (_tcpPanel != null) _tcpPanel.Visible = true;
                    }
                };
            }
        }

        #endregion
    }

    /// <summary>
    /// 连接配置面板构建结果
    /// </summary>
    public class ConnectionPanelBuildResult
    {
        public bool Success { get; set; }
        public RadioButton SerialRadio { get; set; }
        public RadioButton TcpRadio { get; set; }
        public ComboBox PortComboBox { get; set; }
        public ComboBox BaudRateComboBox { get; set; }
        public TextBox IpTextBox { get; set; }
        public NumericUpDown TcpPortNumeric { get; set; }
        public NumericUpDown SlaveIdNumeric { get; set; }
        public Button ConnectButton { get; set; }
        public Button RefreshButton { get; set; }
        public Label StatusLabel { get; set; }
        public Panel SerialPanel { get; set; }
        public Panel TcpPanel { get; set; }
    }
}
