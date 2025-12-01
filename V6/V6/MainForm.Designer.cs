// LEGACY_START
namespace GJVdc32Tool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
namespace GJVdc32Tool
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.pnlSideMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMainContent = new System.Windows.Forms.Panel();
            this.connectionPanel = new GJVdc32Tool.Views.ConnectionPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageMonitor = new System.Windows.Forms.TabPage();
            this.tabPageControl = new System.Windows.Forms.TabPage();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.channelMonitorView = new GJVdc32Tool.Views.Vdc32.ChannelMonitorView();
            this.ioStatusMonitorView = new GJVdc32Tool.Views.Vdc32.IoStatusMonitorView();
            this.ioControlView = new GJVdc32Tool.Views.Vdc32.IoControlView();
            this.environmentView = new GJVdc32Tool.Views.Vdc32.EnvironmentView();
            this.deviceConfigView = new GJVdc32Tool.Views.Vdc32.DeviceConfigView();
            this.logView = new GJVdc32Tool.Views.Shared.LogView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlMainContent.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageMonitor.SuspendLayout();
            this.tabPageControl.SuspendLayout();
            this.tabPageConfig.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.pnlSideMenu);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.pnlMainContent);
            this.splitMain.Size = new System.Drawing.Size(1890, 1050);
            this.splitMain.SplitterDistance = 260;
            this.splitMain.TabIndex = 0;
            // 
            // pnlSideMenu
            // 
            this.pnlSideMenu.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.pnlSideMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSideMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlSideMenu.Name = "pnlSideMenu";
            this.pnlSideMenu.Size = new System.Drawing.Size(260, 1050);
            this.pnlSideMenu.TabIndex = 0;
            // 
            namespace GJVdc32Tool
            {
                partial class MainForm
                {
                    private System.ComponentModel.IContainer components = null;

                    protected override void Dispose(bool disposing)
                    {
                        if (disposing && (components != null))
                        {
                            components.Dispose();
                        }
                        base.Dispose(disposing);
                    }

                    #region Windows 窗体设计器生成的代码

                    private void InitializeComponent()
                    {
                        this.splitMain = new System.Windows.Forms.SplitContainer();
                        this.pnlSideMenu = new System.Windows.Forms.FlowLayoutPanel();
                        this.pnlMainContent = new System.Windows.Forms.Panel();
                        this.connectionPanel = new GJVdc32Tool.Views.ConnectionPanel.ConnectionPanel();
                        this.tabControl = new System.Windows.Forms.TabControl();
                        this.tabPageMonitor = new System.Windows.Forms.TabPage();
                        this.tabPageControl = new System.Windows.Forms.TabPage();
                        this.tabPageConfig = new System.Windows.Forms.TabPage();
                        this.tabPageLog = new System.Windows.Forms.TabPage();
                        this.channelMonitorView = new GJVdc32Tool.Views.Vdc32.ChannelMonitorView();
                        this.ioStatusMonitorView = new GJVdc32Tool.Views.Vdc32.IoStatusMonitorView();
                        this.ioControlView = new GJVdc32Tool.Views.Vdc32.IoControlView();
                        this.environmentView = new GJVdc32Tool.Views.Vdc32.EnvironmentView();
                        this.deviceConfigView = new GJVdc32Tool.Views.Vdc32.DeviceConfigView();
                        this.logView = new GJVdc32Tool.Views.Shared.LogView();
                        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
                        this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
                        ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
                        this.splitMain.Panel1.SuspendLayout();
                        this.splitMain.Panel2.SuspendLayout();
                        this.splitMain.SuspendLayout();
                        this.pnlMainContent.SuspendLayout();
                        this.tabControl.SuspendLayout();
                        this.tabPageMonitor.SuspendLayout();
                        this.tabPageControl.SuspendLayout();
                        this.tabPageConfig.SuspendLayout();
                        this.tabPageLog.SuspendLayout();
                        this.statusStrip1.SuspendLayout();
                        this.SuspendLayout();
                        // 
                        // splitMain
                        // 
                        this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
                        this.splitMain.Location = new System.Drawing.Point(0, 0);
                        this.splitMain.Name = "splitMain";
                        // 
                        // splitMain.Panel1
                        // 
                        this.splitMain.Panel1.Controls.Add(this.pnlSideMenu);
                        // 
                        // splitMain.Panel2
                        // 
                        this.splitMain.Panel2.Controls.Add(this.pnlMainContent);
                        this.splitMain.Size = new System.Drawing.Size(1890, 1050);
                        this.splitMain.SplitterDistance = 260;
                        this.splitMain.TabIndex = 0;
                        // 
                        // pnlSideMenu
                        // 
                        this.pnlSideMenu.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
                        this.pnlSideMenu.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.pnlSideMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
                        this.pnlSideMenu.Location = new System.Drawing.Point(0, 0);
                        this.pnlSideMenu.Name = "pnlSideMenu";
                        this.pnlSideMenu.Size = new System.Drawing.Size(260, 1050);
                        this.pnlSideMenu.TabIndex = 0;
                        // 
                        // pnlMainContent
                        // 
                        this.pnlMainContent.Controls.Add(this.tabControl);
                        this.pnlMainContent.Controls.Add(this.connectionPanel);
                        this.pnlMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.pnlMainContent.Location = new System.Drawing.Point(0, 0);
                        this.pnlMainContent.Name = "pnlMainContent";
                        this.pnlMainContent.Size = new System.Drawing.Size(1626, 1050);
                        this.pnlMainContent.TabIndex = 0;
                        // 
                        // connectionPanel
                        // 
                        this.connectionPanel.Dock = System.Windows.Forms.DockStyle.Top;
                        this.connectionPanel.Location = new System.Drawing.Point(0, 0);
                        this.connectionPanel.Name = "connectionPanel";
                        this.connectionPanel.Size = new System.Drawing.Size(1626, 156);
                        this.connectionPanel.TabIndex = 0;
                        this.connectionPanel.ConnectRequested += new System.EventHandler(this.ConnectionPanel_ConnectRequested);
                        this.connectionPanel.DisconnectRequested += new System.EventHandler(this.ConnectionPanel_DisconnectRequested);
                        // 
                        // tabControl
                        // 
                        this.tabControl.Controls.Add(this.tabPageMonitor);
                        this.tabControl.Controls.Add(this.tabPageControl);
                        this.tabControl.Controls.Add(this.tabPageConfig);
                        this.tabControl.Controls.Add(this.tabPageLog);
                        this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.tabControl.Location = new System.Drawing.Point(0, 156);
                        this.tabControl.Name = "tabControl";
                        this.tabControl.SelectedIndex = 0;
                        this.tabControl.Size = new System.Drawing.Size(1626, 894);
                        this.tabControl.TabIndex = 1;
                        // 
                        // tabPageMonitor
                        // 
                        this.tabPageMonitor.Controls.Add(this.channelMonitorView);
                        this.tabPageMonitor.Controls.Add(this.ioStatusMonitorView);
                        this.tabPageMonitor.Location = new System.Drawing.Point(4, 25);
                        this.tabPageMonitor.Name = "tabPageMonitor";
                        this.tabPageMonitor.Padding = new System.Windows.Forms.Padding(3);
                        this.tabPageMonitor.Size = new System.Drawing.Size(1618, 865);
                        this.tabPageMonitor.TabIndex = 0;
                        this.tabPageMonitor.Text = "监测";
                        this.tabPageMonitor.UseVisualStyleBackColor = true;
                        // 
                        // channelMonitorView
                        // 
                        this.channelMonitorView.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.channelMonitorView.Location = new System.Drawing.Point(3, 90);
                        this.channelMonitorView.Name = "channelMonitorView";
                        this.channelMonitorView.Size = new System.Drawing.Size(1612, 772);
                        this.channelMonitorView.TabIndex = 1;
                        this.channelMonitorView.ClearAllFlagsRequested += new System.EventHandler(this.ChannelMonitorView_ClearAllFlagsRequested);
                        this.channelMonitorView.ExportCsvRequested += new System.EventHandler(this.ChannelMonitorView_ExportCsvRequested);
                        this.channelMonitorView.WriteThresholdsRequested += new System.EventHandler(this.ChannelMonitorView_WriteThresholdsRequested);
                        this.channelMonitorView.ReadThresholdsRequested += new System.EventHandler(this.ChannelMonitorView_ReadThresholdsRequested);
                        this.channelMonitorView.SetAllThresholdRequested += new System.EventHandler<double>(this.ChannelMonitorView_SetAllThresholdRequested);
                        this.channelMonitorView.ChannelClearFlagRequested += new System.EventHandler<int>(this.ChannelMonitorView_ChannelClearFlagRequested);
                        this.channelMonitorView.ChannelWriteThresholdRequested += new System.EventHandler<int>(this.ChannelMonitorView_ChannelWriteThresholdRequested);
                        this.channelMonitorView.ChannelReadThresholdRequested += new System.EventHandler<int>(this.ChannelMonitorView_ChannelReadThresholdRequested);
                        // 
                        // ioStatusMonitorView
                        // 
                        this.ioStatusMonitorView.Dock = System.Windows.Forms.DockStyle.Top;
                        this.ioStatusMonitorView.Location = new System.Drawing.Point(3, 3);
                        this.ioStatusMonitorView.Name = "ioStatusMonitorView";
                        this.ioStatusMonitorView.Size = new System.Drawing.Size(1612, 87);
                        this.ioStatusMonitorView.TabIndex = 0;
                        // 
                        // tabPageControl
                        // 
                        this.tabPageControl.Controls.Add(this.environmentView);
                        this.tabPageControl.Controls.Add(this.ioControlView);
                        this.tabPageControl.Location = new System.Drawing.Point(4, 25);
                        this.tabPageControl.Name = "tabPageControl";
                        this.tabPageControl.Padding = new System.Windows.Forms.Padding(3);
                        this.tabPageControl.Size = new System.Drawing.Size(1618, 865);
                        this.tabPageControl.TabIndex = 1;
                        this.tabPageControl.Text = "控制";
                        this.tabPageControl.UseVisualStyleBackColor = true;
                        // 
                        // environmentView
                        // 
                        this.environmentView.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.environmentView.Location = new System.Drawing.Point(3, 253);
                        this.environmentView.Name = "environmentView";
                        this.environmentView.Size = new System.Drawing.Size(1612, 609);
                        this.environmentView.TabIndex = 1;
                        // 
                        // ioControlView
                        // 
                        this.ioControlView.Dock = System.Windows.Forms.DockStyle.Top;
                        this.ioControlView.Location = new System.Drawing.Point(3, 3);
                        this.ioControlView.Name = "ioControlView";
                        this.ioControlView.Size = new System.Drawing.Size(1612, 250);
                        this.ioControlView.TabIndex = 0;
                        this.ioControlView.IoControlChanged += new System.EventHandler<GJVdc32Tool.Views.Vdc32.IoControlEventArgs>(this.IoControlView_IoControlChanged);
                        // 
                        // tabPageConfig
                        // 
                        this.tabPageConfig.Controls.Add(this.deviceConfigView);
                        this.tabPageConfig.Location = new System.Drawing.Point(4, 25);
                        this.tabPageConfig.Name = "tabPageConfig";
                        this.tabPageConfig.Size = new System.Drawing.Size(1618, 865);
                        this.tabPageConfig.TabIndex = 2;
                        this.tabPageConfig.Text = "配置";
                        this.tabPageConfig.UseVisualStyleBackColor = true;
                        // 
                        // deviceConfigView
                        // 
                        this.deviceConfigView.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.deviceConfigView.Location = new System.Drawing.Point(0, 0);
                        this.deviceConfigView.Name = "deviceConfigView";
                        this.deviceConfigView.Size = new System.Drawing.Size(1618, 865);
                        this.deviceConfigView.TabIndex = 0;
                        // 
                        // tabPageLog
                        // 
                        this.tabPageLog.Controls.Add(this.logView);
                        this.tabPageLog.Location = new System.Drawing.Point(4, 25);
                        this.tabPageLog.Name = "tabPageLog";
                        this.tabPageLog.Size = new System.Drawing.Size(1618, 865);
                        this.tabPageLog.TabIndex = 3;
                        this.tabPageLog.Text = "日志";
                        this.tabPageLog.UseVisualStyleBackColor = true;
                        // 
                        // logView
                        // 
                        this.logView.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.logView.Location = new System.Drawing.Point(0, 0);
                        this.logView.Name = "logView";
                        this.logView.Size = new System.Drawing.Size(1618, 865);
                        this.logView.TabIndex = 0;
                        // 
                        // statusStrip1
                        // 
                        this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                        this.lblStatus});
                        this.statusStrip1.Location = new System.Drawing.Point(0, 1028);
                        this.statusStrip1.Name = "statusStrip1";
                        this.statusStrip1.Size = new System.Drawing.Size(1890, 22);
                        this.statusStrip1.TabIndex = 1;
                        this.statusStrip1.Text = "statusStrip1";
                        // 
                        // lblStatus
                        // 
                        this.lblStatus.Name = "lblStatus";
                        this.lblStatus.Size = new System.Drawing.Size(32, 17);
                        this.lblStatus.Text = "就绪";
                        // 
                        // MainForm
                        // 
                        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
                        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                        this.ClientSize = new System.Drawing.Size(1890, 1050);
                        this.Controls.Add(this.splitMain);
                        this.Controls.Add(this.statusStrip1);
                        this.Name = "MainForm";
                        this.Text = "GJVdc32Tool - VDC-32 & GJDD-750 控制软件";
                        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
                        ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
                        this.splitMain.Panel1.ResumeLayout(false);
                        this.splitMain.Panel2.ResumeLayout(false);
                        this.splitMain.ResumeLayout(false);
                        this.pnlMainContent.ResumeLayout(false);
                        this.tabControl.ResumeLayout(false);
                        this.tabPageMonitor.ResumeLayout(false);
                        this.tabPageControl.ResumeLayout(false);
                        this.tabPageConfig.ResumeLayout(false);
                        this.tabPageLog.ResumeLayout(false);
                        this.statusStrip1.ResumeLayout(false);
                        this.statusStrip1.PerformLayout();
                        this.ResumeLayout(false);
                        this.PerformLayout();
                    }

                    #endregion

                    private System.Windows.Forms.SplitContainer splitMain;
                    private System.Windows.Forms.FlowLayoutPanel pnlSideMenu;
                    private System.Windows.Forms.Panel pnlMainContent;
                    private Views.ConnectionPanel.ConnectionPanel connectionPanel;
                    private System.Windows.Forms.TabControl tabControl;
                    private System.Windows.Forms.TabPage tabPageMonitor;
                    private System.Windows.Forms.TabPage tabPageControl;
                    private System.Windows.Forms.TabPage tabPageConfig;
                    private System.Windows.Forms.TabPage tabPageLog;
                    private Views.Vdc32.ChannelMonitorView channelMonitorView;
                    private Views.Vdc32.IoStatusMonitorView ioStatusMonitorView;
                    private Views.Vdc32.IoControlView ioControlView;
                    private Views.Vdc32.EnvironmentView environmentView;
                    private Views.Vdc32.DeviceConfigView deviceConfigView;
                    private Views.Shared.LogView logView;
                    private System.Windows.Forms.StatusStrip statusStrip1;
                    private System.Windows.Forms.ToolStripStatusLabel lblStatus;
                }
            }
            // LEGACY_START
            // channelMonitorView
            // 
            this.channelMonitorView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.channelMonitorView.Location = new System.Drawing.Point(3, 81);
            this.channelMonitorView.Name = "channelMonitorView";
            this.channelMonitorView.Size = new System.Drawing.Size(1612, 781);
            this.channelMonitorView.TabIndex = 1;
            // 
            // ioStatusMonitorView
            // 
            this.ioStatusMonitorView.Dock = System.Windows.Forms.DockStyle.Top;
            this.ioStatusMonitorView.Location = new System.Drawing.Point(3, 3);
            this.ioStatusMonitorView.Name = "ioStatusMonitorView";
            this.ioStatusMonitorView.Size = new System.Drawing.Size(1612, 78);
            this.ioStatusMonitorView.TabIndex = 0;
            // 
            // tabPageControl
            // 
            this.tabPageControl.Controls.Add(this.environmentView);
            this.tabPageControl.Controls.Add(this.ioControlView);
            this.tabPageControl.Location = new System.Drawing.Point(4, 25);
            this.tabPageControl.Name = "tabPageControl";
            this.tabPageControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageControl.Size = new System.Drawing.Size(1618, 865);
            this.tabPageControl.TabIndex = 1;
            this.tabPageControl.Text = "控制";
            this.tabPageControl.UseVisualStyleBackColor = true;
            // 
            // environmentView
            // 
            this.environmentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.environmentView.Location = new System.Drawing.Point(3, 253);
            this.environmentView.Name = "environmentView";
            this.environmentView.Size = new System.Drawing.Size(1612, 609);
            this.environmentView.TabIndex = 1;
            // 
            // ioControlView
            // 
            this.ioControlView.Dock = System.Windows.Forms.DockStyle.Top;
            this.ioControlView.Location = new System.Drawing.Point(3, 3);
            this.ioControlView.Name = "ioControlView";
            this.ioControlView.Size = new System.Drawing.Size(1612, 250);
            this.ioControlView.TabIndex = 0;
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Controls.Add(this.deviceConfigView);
            this.tabPageConfig.Location = new System.Drawing.Point(4, 25);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Size = new System.Drawing.Size(1618, 865);
            this.tabPageConfig.TabIndex = 2;
            this.tabPageConfig.Text = "配置";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // deviceConfigView
            // 
            this.deviceConfigView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceConfigView.Location = new System.Drawing.Point(0, 0);
            this.deviceConfigView.Name = "deviceConfigView";
            this.deviceConfigView.Size = new System.Drawing.Size(1618, 865);
            this.deviceConfigView.TabIndex = 0;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.logView);
            this.tabPageLog.Location = new System.Drawing.Point(4, 25);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Size = new System.Drawing.Size(1618, 865);
            this.tabPageLog.TabIndex = 3;
            this.tabPageLog.Text = "日志";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // logView
            // 
            this.logView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logView.Location = new System.Drawing.Point(0, 0);
            this.logView.Name = "logView";
            this.logView.Size = new System.Drawing.Size(1618, 865);
            this.logView.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1028);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1890, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(32, 17);
            this.lblStatus.Text = "就绪";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1890, 1050);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "GJVdc32Tool - VDC-32 & GJDD-750 控制软件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.ResumeLayout(false);
            this.pnlMainContent.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageMonitor.ResumeLayout(false);
            this.tabPageControl.ResumeLayout(false);
            this.tabPageConfig.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.FlowLayoutPanel pnlSideMenu;
        private System.Windows.Forms.Panel pnlMainContent;
        private Views.ConnectionPanel connectionPanel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageMonitor;
        private System.Windows.Forms.TabPage tabPageControl;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.TabPage tabPageLog;
        private Views.Vdc32.ChannelMonitorView channelMonitorView;
        private Views.Vdc32.IoStatusMonitorView ioStatusMonitorView;
        private Views.Vdc32.IoControlView ioControlView;
        private Views.Vdc32.EnvironmentView environmentView;
        private Views.Vdc32.DeviceConfigView deviceConfigView;
        private Views.Shared.LogView logView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";
        }

        #endregion
    }
}