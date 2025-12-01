namespace GJVdc32Tool.Views
{
    partial class ConnectionPanel
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox groupBoxConnect;
        private System.Windows.Forms.GroupBox groupBoxDeviceInfo;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblDeviceAddress;
        private System.Windows.Forms.Label lblDeviceName;
        private System.Windows.Forms.Label lblDeviceVersion;
        private System.Windows.Forms.RadioButton radioSerial;
        private System.Windows.Forms.RadioButton radioTcp;
        private System.Windows.Forms.Button btnRefreshPorts;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.NumericUpDown numSlaveId;
        private System.Windows.Forms.Label labelSlaveId;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Label labelBaudRate;
        private System.Windows.Forms.ComboBox cmbComPorts;
        private System.Windows.Forms.Label labelComPort;
        private System.Windows.Forms.TextBox txtTcpIp;
        private System.Windows.Forms.Label labelTcpIp;
        private System.Windows.Forms.NumericUpDown numTcpPort;
        private System.Windows.Forms.Label labelTcpPort;
        private System.Windows.Forms.Button btnApplySlaveId;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBoxConnect = new System.Windows.Forms.GroupBox();
            this.btnApplySlaveId = new System.Windows.Forms.Button();
            this.groupBoxDeviceInfo = new System.Windows.Forms.GroupBox();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblDeviceAddress = new System.Windows.Forms.Label();
            this.lblDeviceName = new System.Windows.Forms.Label();
            this.lblDeviceVersion = new System.Windows.Forms.Label();
            this.radioSerial = new System.Windows.Forms.RadioButton();
            this.radioTcp = new System.Windows.Forms.RadioButton();
            this.btnRefreshPorts = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.numSlaveId = new System.Windows.Forms.NumericUpDown();
            this.labelSlaveId = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.labelBaudRate = new System.Windows.Forms.Label();
            this.cmbComPorts = new System.Windows.Forms.ComboBox();
            this.labelComPort = new System.Windows.Forms.Label();
            this.txtTcpIp = new System.Windows.Forms.TextBox();
            this.labelTcpIp = new System.Windows.Forms.Label();
            this.numTcpPort = new System.Windows.Forms.NumericUpDown();
            this.labelTcpPort = new System.Windows.Forms.Label();
            this.groupBoxConnect.SuspendLayout();
            this.groupBoxDeviceInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSlaveId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxConnect
            // 
            this.groupBoxConnect.Controls.Add(this.btnApplySlaveId);
            this.groupBoxConnect.Controls.Add(this.groupBoxDeviceInfo);
            this.groupBoxConnect.Controls.Add(this.radioSerial);
            this.groupBoxConnect.Controls.Add(this.radioTcp);
            this.groupBoxConnect.Controls.Add(this.btnRefreshPorts);
            this.groupBoxConnect.Controls.Add(this.btnConnect);
            this.groupBoxConnect.Controls.Add(this.numSlaveId);
            this.groupBoxConnect.Controls.Add(this.labelSlaveId);
            this.groupBoxConnect.Controls.Add(this.cmbBaudRate);
            this.groupBoxConnect.Controls.Add(this.labelBaudRate);
            this.groupBoxConnect.Controls.Add(this.cmbComPorts);
            this.groupBoxConnect.Controls.Add(this.labelComPort);
            this.groupBoxConnect.Controls.Add(this.txtTcpIp);
            this.groupBoxConnect.Controls.Add(this.labelTcpIp);
            this.groupBoxConnect.Controls.Add(this.numTcpPort);
            this.groupBoxConnect.Controls.Add(this.labelTcpPort);
            this.groupBoxConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxConnect.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxConnect.Location = new System.Drawing.Point(0, 0);
            this.groupBoxConnect.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxConnect.Name = "groupBoxConnect";
            this.groupBoxConnect.Padding = new System.Windows.Forms.Padding(5);
            this.groupBoxConnect.Size = new System.Drawing.Size(1100, 150);
            this.groupBoxConnect.TabIndex = 0;
            this.groupBoxConnect.TabStop = false;
            this.groupBoxConnect.Text = "● 连接配置";
            // 
            // btnApplySlaveId
            // 
            this.btnApplySlaveId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnApplySlaveId.FlatAppearance.BorderSize = 0;
            this.btnApplySlaveId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplySlaveId.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Bold);
            this.btnApplySlaveId.ForeColor = System.Drawing.Color.White;
            this.btnApplySlaveId.Location = new System.Drawing.Point(770, 90);
            this.btnApplySlaveId.Name = "btnApplySlaveId";
            this.btnApplySlaveId.Size = new System.Drawing.Size(75, 27);
            this.btnApplySlaveId.TabIndex = 21;
            this.btnApplySlaveId.Text = "应用";
            this.btnApplySlaveId.UseVisualStyleBackColor = false;
            this.btnApplySlaveId.Click += new System.EventHandler(this.btnApplySlaveId_Click);
            // 
            // groupBoxDeviceInfo
            // 
            this.groupBoxDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDeviceInfo.Controls.Add(this.lblConnectionStatus);
            this.groupBoxDeviceInfo.Controls.Add(this.lblDeviceAddress);
            this.groupBoxDeviceInfo.Controls.Add(this.lblDeviceName);
            this.groupBoxDeviceInfo.Controls.Add(this.lblDeviceVersion);
            this.groupBoxDeviceInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxDeviceInfo.Location = new System.Drawing.Point(880, 26);
            this.groupBoxDeviceInfo.Name = "groupBoxDeviceInfo";
            this.groupBoxDeviceInfo.Size = new System.Drawing.Size(208, 110);
            this.groupBoxDeviceInfo.TabIndex = 20;
            this.groupBoxDeviceInfo.TabStop = false;
            this.groupBoxDeviceInfo.Text = "📌 板卡信息";
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
            this.lblConnectionStatus.Location = new System.Drawing.Point(12, 80);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(68, 19);
            this.lblConnectionStatus.TabIndex = 3;
            this.lblConnectionStatus.Text = "● 未连接";
            // 
            // lblDeviceAddress
            // 
            this.lblDeviceAddress.AutoSize = true;
            this.lblDeviceAddress.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDeviceAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblDeviceAddress.Location = new System.Drawing.Point(12, 60);
            this.lblDeviceAddress.Name = "lblDeviceAddress";
            this.lblDeviceAddress.Size = new System.Drawing.Size(89, 20);
            this.lblDeviceAddress.TabIndex = 2;
            this.lblDeviceAddress.Text = "从机地址: --";
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.AutoSize = true;
            this.lblDeviceName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDeviceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblDeviceName.Location = new System.Drawing.Point(12, 40);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(89, 20);
            this.lblDeviceName.TabIndex = 1;
            this.lblDeviceName.Text = "设备名称: --";
            // 
            // lblDeviceVersion
            // 
            this.lblDeviceVersion.AutoSize = true;
            this.lblDeviceVersion.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDeviceVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblDeviceVersion.Location = new System.Drawing.Point(12, 20);
            this.lblDeviceVersion.Name = "lblDeviceVersion";
            this.lblDeviceVersion.Size = new System.Drawing.Size(89, 20);
            this.lblDeviceVersion.TabIndex = 0;
            this.lblDeviceVersion.Text = "固件版本: --";
            // 
            // radioSerial
            // 
            this.radioSerial.AutoSize = true;
            this.radioSerial.Checked = true;
            this.radioSerial.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.radioSerial.Location = new System.Drawing.Point(20, 40);
            this.radioSerial.Name = "radioSerial";
            this.radioSerial.Size = new System.Drawing.Size(99, 27);
            this.radioSerial.TabIndex = 10;
            this.radioSerial.TabStop = true;
            this.radioSerial.Text = "串口连接";
            this.radioSerial.UseVisualStyleBackColor = true;
            this.radioSerial.CheckedChanged += new System.EventHandler(this.radioSerial_CheckedChanged);
            // 
            // radioTcp
            // 
            this.radioTcp.AutoSize = true;
            this.radioTcp.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.radioTcp.Location = new System.Drawing.Point(140, 40);
            this.radioTcp.Name = "radioTcp";
            this.radioTcp.Size = new System.Drawing.Size(96, 27);
            this.radioTcp.TabIndex = 11;
            this.radioTcp.Text = "TCP连接";
            this.radioTcp.UseVisualStyleBackColor = true;
            // 
            // btnRefreshPorts
            // 
            this.btnRefreshPorts.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRefreshPorts.Location = new System.Drawing.Point(220, 85);
            this.btnRefreshPorts.Name = "btnRefreshPorts";
            this.btnRefreshPorts.Size = new System.Drawing.Size(75, 32);
            this.btnRefreshPorts.TabIndex = 1;
            this.btnRefreshPorts.Text = "刷新";
            this.btnRefreshPorts.UseVisualStyleBackColor = true;
            this.btnRefreshPorts.Click += new System.EventHandler(this.btnRefreshPorts_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(850, 82);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(120, 40);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // numSlaveId
            // 
            this.numSlaveId.Font = new System.Drawing.Font("Consolas", 10F);
            this.numSlaveId.Location = new System.Drawing.Point(680, 90);
            this.numSlaveId.Maximum = new decimal(new int[] {
            247,
            0,
            0,
            0});
            this.numSlaveId.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSlaveId.Name = "numSlaveId";
            this.numSlaveId.Size = new System.Drawing.Size(70, 27);
            this.numSlaveId.TabIndex = 3;
            this.numSlaveId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSlaveId.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelSlaveId
            // 
            this.labelSlaveId.AutoSize = true;
            this.labelSlaveId.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelSlaveId.Location = new System.Drawing.Point(600, 94);
            this.labelSlaveId.Name = "labelSlaveId";
            this.labelSlaveId.Size = new System.Drawing.Size(84, 20);
            this.labelSlaveId.TabIndex = 5;
            this.labelSlaveId.Text = "从机地址：";
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBaudRate.Font = new System.Drawing.Font("Consolas", 10F);
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Location = new System.Drawing.Point(380, 90);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(140, 28);
            this.cmbBaudRate.TabIndex = 2;
            // 
            // labelBaudRate
            // 
            this.labelBaudRate.AutoSize = true;
            this.labelBaudRate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelBaudRate.Location = new System.Drawing.Point(320, 94);
            this.labelBaudRate.Name = "labelBaudRate";
            this.labelBaudRate.Size = new System.Drawing.Size(69, 20);
            this.labelBaudRate.TabIndex = 2;
            this.labelBaudRate.Text = "波特率：";
            // 
            // cmbComPorts
            // 
            this.cmbComPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComPorts.Font = new System.Drawing.Font("Consolas", 10F);
            this.cmbComPorts.FormattingEnabled = true;
            this.cmbComPorts.Location = new System.Drawing.Point(80, 90);
            this.cmbComPorts.Name = "cmbComPorts";
            this.cmbComPorts.Size = new System.Drawing.Size(130, 28);
            this.cmbComPorts.TabIndex = 0;
            // 
            // labelComPort
            // 
            this.labelComPort.AutoSize = true;
            this.labelComPort.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelComPort.Location = new System.Drawing.Point(20, 94);
            this.labelComPort.Name = "labelComPort";
            this.labelComPort.Size = new System.Drawing.Size(54, 20);
            this.labelComPort.TabIndex = 0;
            this.labelComPort.Text = "串口：";
            // 
            // txtTcpIp
            // 
            this.txtTcpIp.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtTcpIp.Location = new System.Drawing.Point(110, 90);
            this.txtTcpIp.Name = "txtTcpIp";
            this.txtTcpIp.Size = new System.Drawing.Size(180, 27);
            this.txtTcpIp.TabIndex = 12;
            this.txtTcpIp.Text = "192.168.1.100";
            this.txtTcpIp.Visible = false;
            // 
            // labelTcpIp
            // 
            this.labelTcpIp.AutoSize = true;
            this.labelTcpIp.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelTcpIp.Location = new System.Drawing.Point(20, 94);
            this.labelTcpIp.Name = "labelTcpIp";
            this.labelTcpIp.Size = new System.Drawing.Size(67, 20);
            this.labelTcpIp.TabIndex = 13;
            this.labelTcpIp.Text = "IP地址：";
            this.labelTcpIp.Visible = false;
            // 
            // numTcpPort
            // 
            this.numTcpPort.Font = new System.Drawing.Font("Consolas", 10F);
            this.numTcpPort.Location = new System.Drawing.Point(380, 90);
            this.numTcpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTcpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTcpPort.Name = "numTcpPort";
            this.numTcpPort.Size = new System.Drawing.Size(140, 27);
            this.numTcpPort.TabIndex = 14;
            this.numTcpPort.Value = new decimal(new int[] {
            502,
            0,
            0,
            0});
            this.numTcpPort.Visible = false;
            // 
            // labelTcpPort
            // 
            this.labelTcpPort.AutoSize = true;
            this.labelTcpPort.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelTcpPort.Location = new System.Drawing.Point(320, 94);
            this.labelTcpPort.Name = "labelTcpPort";
            this.labelTcpPort.Size = new System.Drawing.Size(54, 20);
            this.labelTcpPort.TabIndex = 15;
            this.labelTcpPort.Text = "端口：";
            this.labelTcpPort.Visible = false;
            // 
            // ConnectionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupBoxConnect);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ConnectionPanel";
            this.Size = new System.Drawing.Size(1100, 150);
            this.groupBoxConnect.ResumeLayout(false);
            this.groupBoxConnect.PerformLayout();
            this.groupBoxDeviceInfo.ResumeLayout(false);
            this.groupBoxDeviceInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSlaveId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
