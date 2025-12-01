using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace GJVdc32Tool.Views
{
    public partial class ConnectionPanel : UserControl
    {
        public event EventHandler ConnectRequested;
        public event EventHandler DisconnectRequested;
        public event EventHandler<ConnectionConfigEventArgs> ConfigChanged;

        private bool _isConnected;

        public ConnectionPanel()
        {
            InitializeComponent();
            InitializeComPorts();
            InitializeBaudRates();
            HookConfigChangeEvents();
        }

        #region 公共属性

        public bool IsSerialMode => radioSerial.Checked;
        public string SelectedPort => cmbComPorts.Text;
        public int SelectedBaudRate => (int)(cmbBaudRate.SelectedItem ?? 57600);
        public byte SlaveId => (byte)numSlaveId.Value;
        public string TcpIp => txtTcpIp.Text;
        public int TcpPort => (int)numTcpPort.Value;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected == value)
                    return;

                _isConnected = value;
                UpdateUIState(value);
            }
        }

        #endregion

        #region 公共方法

        public void SetDeviceInfo(string version, string name, byte address)
        {
            lblDeviceVersion.Text = $"固件版本: {version}";
            lblDeviceName.Text = $"设备名称: {name}";
            lblDeviceAddress.Text = $"从机地址: {address}";
        }

        public void SetConnectionStatus(bool connected, string statusText = null)
        {
            lblConnectionStatus.Text = statusText ?? (connected ? "● 已连接" : "● 未连接");
            lblConnectionStatus.ForeColor = connected
                ? Color.FromArgb(76, 175, 80)
                : Color.FromArgb(158, 158, 158);
        }

        public void RefreshComPorts()
        {
            string oldPort = cmbComPorts.Text;
            cmbComPorts.Items.Clear();
            cmbComPorts.Items.AddRange(SerialPort.GetPortNames());

            if (cmbComPorts.Items.Contains(oldPort))
            {
                cmbComPorts.SelectedItem = oldPort;
            }
            else if (cmbComPorts.Items.Count > 0)
            {
                cmbComPorts.SelectedIndex = 0;
            }
        }

        #endregion

        #region 私有方法

        private void InitializeComPorts()
        {
            cmbComPorts.Items.AddRange(SerialPort.GetPortNames());
            if (cmbComPorts.Items.Count > 0)
            {
                cmbComPorts.SelectedIndex = 0;
            }
        }

        private void InitializeBaudRates()
        {
            cmbBaudRate.Items.AddRange(new object[] { 9600, 19200, 38400, 57600, 115200 });
            cmbBaudRate.SelectedItem = 57600;
        }

        private void UpdateUIState(bool isConnected)
        {
            radioSerial.Enabled = !isConnected;
            radioTcp.Enabled = !isConnected;
            cmbComPorts.Enabled = !isConnected;
            cmbBaudRate.Enabled = !isConnected;
            btnRefreshPorts.Enabled = !isConnected;
            txtTcpIp.Enabled = !isConnected;
            numTcpPort.Enabled = !isConnected;
            numSlaveId.Enabled = !isConnected;

            btnConnect.Text = isConnected ? "断开" : "连接";
            btnConnect.BackColor = isConnected
                ? Color.FromArgb(76, 175, 80)
                : Color.FromArgb(244, 67, 54);
        }

        private void HookConfigChangeEvents()
        {
            radioSerial.CheckedChanged += (s, e) => RaiseConfigChanged();
            radioTcp.CheckedChanged += (s, e) => RaiseConfigChanged();
            cmbComPorts.SelectedIndexChanged += (s, e) => RaiseConfigChanged();
            cmbBaudRate.SelectedIndexChanged += (s, e) => RaiseConfigChanged();
            numSlaveId.ValueChanged += (s, e) => RaiseConfigChanged();
            txtTcpIp.TextChanged += (s, e) => RaiseConfigChanged();
            numTcpPort.ValueChanged += (s, e) => RaiseConfigChanged();
        }

        private void RaiseConfigChanged()
        {
            ConfigChanged?.Invoke(this, new ConnectionConfigEventArgs
            {
                UseSerial = IsSerialMode,
                Port = SelectedPort,
                BaudRate = SelectedBaudRate,
                SlaveId = SlaveId,
                TcpIp = TcpIp,
                TcpPort = TcpPort
            });
        }

        #endregion

        #region 事件处理

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                DisconnectRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ConnectRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void btnRefreshPorts_Click(object sender, EventArgs e)
        {
            RefreshComPorts();
        }

        private void radioSerial_CheckedChanged(object sender, EventArgs e)
        {
            bool isSerial = radioSerial.Checked;

            labelComPort.Visible = isSerial;
            cmbComPorts.Visible = isSerial;
            btnRefreshPorts.Visible = isSerial;
            labelBaudRate.Visible = isSerial;
            cmbBaudRate.Visible = isSerial;

            labelTcpIp.Visible = !isSerial;
            txtTcpIp.Visible = !isSerial;
            labelTcpPort.Visible = !isSerial;
            numTcpPort.Visible = !isSerial;
        }

        private void btnApplySlaveId_Click(object sender, EventArgs e)
        {
            RaiseConfigChanged();
        }

        #endregion
    }

    public class ConnectionConfigEventArgs : EventArgs
    {
        public bool UseSerial { get; set; }
        public string Port { get; set; }
        public int BaudRate { get; set; }
        public byte SlaveId { get; set; }
        public string TcpIp { get; set; }
        public int TcpPort { get; set; }
    }
}
