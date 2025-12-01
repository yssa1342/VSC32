using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Views
{
    /// <summary>
    /// 连接配置面板控件
    /// 职责：串口/TCP 连接配置 UI
    /// </summary>
    public partial class ConnectionPanelView : UserControl, IConnectionPanel
    {
        #region 常量定义

        private static readonly int[] BAUD_RATES = { 9600, 19200, 38400, 57600, 115200 };
        private const int DEFAULT_TCP_PORT = 502;

        #endregion

        #region 私有字段

        private Label _lblTitle;
        private RadioButton _rdoSerial;
        private RadioButton _rdoTcp;
        private Panel _serialPanel;
        private Panel _tcpPanel;
        private ComboBox _cmbPort;
        private ComboBox _cmbBaudRate;
        private TextBox _txtIp;
        private NumericUpDown _numTcpPort;
        private NumericUpDown _numSlaveId;
        private Panel _slaveIdPanel;
        private Button _btnConnect;
        private Button _btnRefresh;
        private Label _lblStatus;

        #endregion

        #region 事件

        public event EventHandler ConnectRequested;
        public event EventHandler RefreshPortsRequested;

        #endregion

        #region 属性

        public string PanelTitle
        {
            get => _lblTitle?.Text ?? string.Empty;
            set { if (_lblTitle != null) _lblTitle.Text = value; }
        }

        public bool IsTcpMode => _rdoTcp?.Checked ?? false;

        public string SelectedPort => _cmbPort?.SelectedItem?.ToString() ?? string.Empty;

        public int BaudRate
        {
            get
            {
                if (_cmbBaudRate?.SelectedItem != null)
                    return (int)_cmbBaudRate.SelectedItem;
                return 9600;
            }
        }

        public string IpAddress => _txtIp?.Text ?? string.Empty;

        public int TcpPort => (int)(_numTcpPort?.Value ?? DEFAULT_TCP_PORT);

        public byte SlaveId => (byte)(_numSlaveId?.Value ?? 1);

        public bool SerialControlsEnabled
        {
            get => _cmbPort?.Enabled ?? false;
            set
            {
                if (_cmbPort != null) _cmbPort.Enabled = value;
                if (_cmbBaudRate != null) _cmbBaudRate.Enabled = value;
                if (_btnRefresh != null) _btnRefresh.Enabled = value;
            }
        }

        public bool TcpControlsEnabled
        {
            get => _txtIp?.Enabled ?? false;
            set
            {
                if (_txtIp != null) _txtIp.Enabled = value;
                if (_numTcpPort != null) _numTcpPort.Enabled = value;
            }
        }

        public bool SerialOptionEnabled
        {
            get => _rdoSerial?.Enabled ?? false;
            set { if (_rdoSerial != null) _rdoSerial.Enabled = value; }
        }

        public bool TcpOptionEnabled
        {
            get => _rdoTcp?.Enabled ?? false;
            set
            {
                if (_rdoTcp != null)
                {
                    _rdoTcp.Enabled = value;
                    if (!value && _rdoTcp.Checked)
                    {
                        _rdoSerial.Checked = true;
                    }
                }
            }
        }

        public bool SlaveIdVisible
        {
            get => _slaveIdPanel?.Visible ?? false;
            set { if (_slaveIdPanel != null) _slaveIdPanel.Visible = value; }
        }

        public string ConnectionStatusText
        {
            get => _lblStatus?.Text ?? string.Empty;
            set { if (_lblStatus != null) _lblStatus.Text = value; }
        }

        public Color ConnectionStatusColor
        {
            get => _lblStatus?.ForeColor ?? Color.Gray;
            set { if (_lblStatus != null) _lblStatus.ForeColor = value; }
        }

        public string ConnectButtonText
        {
            get => _btnConnect?.Text ?? string.Empty;
            set { if (_btnConnect != null) _btnConnect.Text = value; }
        }

        public Color ConnectButtonColor
        {
            get => _btnConnect?.BackColor ?? Color.Gray;
            set { if (_btnConnect != null) _btnConnect.BackColor = value; }
        }

        #endregion

        #region 构造函数

        public ConnectionPanelView()
        {
            InitializeComponent();
            BuildUI();
            RefreshPorts();
        }

        #endregion

        #region IConnectionPanel 实现

        public void RefreshPorts()
        {
            if (_cmbPort == null) return;

            _cmbPort.Items.Clear();
            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                _cmbPort.Items.Add(port);
            }
            if (_cmbPort.Items.Count > 0)
            {
                _cmbPort.SelectedIndex = 0;
            }
        }

        public void SetConnected(bool isConnected)
        {
            InvokeIfRequired(() =>
            {
                if (isConnected)
                {
                    ConnectionStatusText = "● 已连接";
                    ConnectionStatusColor = Color.FromArgb(76, 175, 80);
                    ConnectButtonText = "断开";
                    ConnectButtonColor = Color.FromArgb(244, 67, 54);
                    SerialControlsEnabled = false;
                    TcpControlsEnabled = false;
                    SerialOptionEnabled = false;
                    TcpOptionEnabled = false;
                }
                else
                {
                    ConnectionStatusText = "● 未连接";
                    ConnectionStatusColor = Color.FromArgb(158, 158, 158);
                    ConnectButtonText = "连接";
                    ConnectButtonColor = Color.FromArgb(33, 150, 243);
                    SerialControlsEnabled = true;
                    TcpControlsEnabled = true;
                    SerialOptionEnabled = true;
                    TcpOptionEnabled = true;
                }
            });
        }

        #endregion

        #region 私有方法

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Name = "ConnectionPanelView";
            this.Size = new Size(280, 250);
            this.ResumeLayout(false);
        }

        private void BuildUI()
        {
            this.SuspendLayout();

            int yPos = 10;

            // 标题
            _lblTitle = new Label
            {
                Text = "● 连接配置",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                Location = new Point(10, yPos),
                AutoSize = true
            };
            this.Controls.Add(_lblTitle);
            yPos += 30;

            // 连接类型
            var typePanel = CreateTypePanel();
            typePanel.Location = new Point(10, yPos);
            this.Controls.Add(typePanel);
            yPos += 30;

            // 串口配置
            _serialPanel = CreateSerialPanel();
            _serialPanel.Location = new Point(10, yPos);
            this.Controls.Add(_serialPanel);

            // TCP 配置
            _tcpPanel = CreateTcpPanel();
            _tcpPanel.Location = new Point(10, yPos);
            _tcpPanel.Visible = false;
            this.Controls.Add(_tcpPanel);
            yPos += 65;

            // 从机地址
            _slaveIdPanel = CreateSlaveIdPanel();
            _slaveIdPanel.Location = new Point(10, yPos);
            this.Controls.Add(_slaveIdPanel);
            yPos += 35;

            // 操作区
            var actionPanel = CreateActionPanel();
            actionPanel.Location = new Point(10, yPos);
            this.Controls.Add(actionPanel);

            this.ResumeLayout(true);
        }

        private Panel CreateTypePanel()
        {
            var panel = new Panel { Size = new Size(260, 25) };

            _rdoSerial = new RadioButton
            {
                Text = "串口",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(0, 2),
                AutoSize = true,
                Checked = true
            };
            _rdoSerial.CheckedChanged += OnConnectionTypeChanged;
            panel.Controls.Add(_rdoSerial);

            _rdoTcp = new RadioButton
            {
                Text = "TCP/IP",
                Font = new Font("Segoe UI", 9f),
                Location = new Point(80, 2),
                AutoSize = true
            };
            panel.Controls.Add(_rdoTcp);

            return panel;
        }

        private Panel CreateSerialPanel()
        {
            var panel = new Panel { Size = new Size(260, 60) };

            // 串口
            panel.Controls.Add(CreateLabel("串口:", 0, 3));
            _cmbPort = new ComboBox
            {
                Location = new Point(45, 0),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panel.Controls.Add(_cmbPort);

            _btnRefresh = new Button
            {
                Text = "↻",
                Font = new Font("Segoe UI", 10f),
                Location = new Point(150, 0),
                Size = new Size(28, 24),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnRefresh.Click += (s, e) =>
            {
                RefreshPorts();
                RefreshPortsRequested?.Invoke(this, EventArgs.Empty);
            };
            panel.Controls.Add(_btnRefresh);

            // 波特率
            panel.Controls.Add(CreateLabel("波特率:", 0, 33));
            _cmbBaudRate = new ComboBox
            {
                Location = new Point(50, 30),
                Size = new Size(95, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var baud in BAUD_RATES)
                _cmbBaudRate.Items.Add(baud);
            _cmbBaudRate.SelectedItem = 9600;
            panel.Controls.Add(_cmbBaudRate);

            return panel;
        }

        private Panel CreateTcpPanel()
        {
            var panel = new Panel { Size = new Size(260, 60) };

            // IP
            panel.Controls.Add(CreateLabel("IP:", 0, 3));
            _txtIp = new TextBox
            {
                Location = new Point(25, 0),
                Size = new Size(150, 25),
                Text = "192.168.1.100"
            };
            panel.Controls.Add(_txtIp);

            // 端口
            panel.Controls.Add(CreateLabel("端口:", 0, 33));
            _numTcpPort = new NumericUpDown
            {
                Location = new Point(40, 30),
                Size = new Size(70, 25),
                Minimum = 1,
                Maximum = 65535,
                Value = DEFAULT_TCP_PORT
            };
            panel.Controls.Add(_numTcpPort);

            return panel;
        }

        private Panel CreateSlaveIdPanel()
        {
            var panel = new Panel { Size = new Size(200, 28) };

            panel.Controls.Add(CreateLabel("从机地址:", 0, 5));
            _numSlaveId = new NumericUpDown
            {
                Location = new Point(65, 2),
                Size = new Size(55, 25),
                Minimum = 1,
                Maximum = 247,
                Value = 1
            };
            panel.Controls.Add(_numSlaveId);

            return panel;
        }

        private Panel CreateActionPanel()
        {
            var panel = new Panel { Size = new Size(260, 35) };

            _btnConnect = new Button
            {
                Text = "连接",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Size = new Size(70, 30),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnConnect.Click += (s, e) => ConnectRequested?.Invoke(this, EventArgs.Empty);
            panel.Controls.Add(_btnConnect);

            _lblStatus = new Label
            {
                Text = "● 未连接",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(158, 158, 158),
                Location = new Point(80, 8),
                AutoSize = true
            };
            panel.Controls.Add(_lblStatus);

            return panel;
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9f),
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private void OnConnectionTypeChanged(object sender, EventArgs e)
        {
            bool isSerial = _rdoSerial.Checked;
            _serialPanel.Visible = isSerial;
            _tcpPanel.Visible = !isSerial;
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
