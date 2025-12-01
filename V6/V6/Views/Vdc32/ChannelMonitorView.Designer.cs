namespace GJVdc32Tool.Views.Vdc32
{
    partial class ChannelMonitorView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Button btnClearAllFlags;
        private System.Windows.Forms.Button btnReadThresholds;
        private System.Windows.Forms.Button btnWriteThresholds;
        private System.Windows.Forms.Button btnSetAllThresholds;
        private System.Windows.Forms.TextBox txtSetAllThresholds;
        private System.Windows.Forms.DataGridView dgvChannels;

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
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.txtSetAllThresholds = new System.Windows.Forms.TextBox();
            this.btnSetAllThresholds = new System.Windows.Forms.Button();
            this.btnWriteThresholds = new System.Windows.Forms.Button();
            this.btnReadThresholds = new System.Windows.Forms.Button();
            this.btnClearAllFlags = new System.Windows.Forms.Button();
            this.dgvChannels = new System.Windows.Forms.DataGridView();
            this.groupBoxMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.btnExportCsv);
            this.groupBoxMain.Controls.Add(this.txtSetAllThresholds);
            this.groupBoxMain.Controls.Add(this.btnSetAllThresholds);
            this.groupBoxMain.Controls.Add(this.btnWriteThresholds);
            this.groupBoxMain.Controls.Add(this.btnReadThresholds);
            this.groupBoxMain.Controls.Add(this.btnClearAllFlags);
            this.groupBoxMain.Controls.Add(this.dgvChannels);
            this.groupBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMain.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxMain.Size = new System.Drawing.Size(1100, 600);
            this.groupBoxMain.TabIndex = 6;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "32通道电压监测与跌落检测";
            // 
            // btnExportCsv
            // 
            this.btnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportCsv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnExportCsv.FlatAppearance.BorderSize = 0;
            this.btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCsv.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportCsv.ForeColor = System.Drawing.Color.White;
            this.btnExportCsv.Location = new System.Drawing.Point(220, 533);
            this.btnExportCsv.Margin = new System.Windows.Forms.Padding(5);
            this.btnExportCsv.Name = "btnExportCsv";
            this.btnExportCsv.Size = new System.Drawing.Size(200, 45);
            this.btnExportCsv.TabIndex = 2;
            this.btnExportCsv.Text = "导出数据表格";
            this.btnExportCsv.UseVisualStyleBackColor = false;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // txtSetAllThresholds
            // 
            this.txtSetAllThresholds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSetAllThresholds.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtSetAllThresholds.Location = new System.Drawing.Point(850, 559);
            this.txtSetAllThresholds.Margin = new System.Windows.Forms.Padding(5);
            this.txtSetAllThresholds.Name = "txtSetAllThresholds";
            this.txtSetAllThresholds.Size = new System.Drawing.Size(99, 27);
            this.txtSetAllThresholds.TabIndex = 4;
            // 
            // btnSetAllThresholds
            // 
            this.btnSetAllThresholds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetAllThresholds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnSetAllThresholds.FlatAppearance.BorderSize = 0;
            this.btnSetAllThresholds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetAllThresholds.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnSetAllThresholds.ForeColor = System.Drawing.Color.White;
            this.btnSetAllThresholds.Location = new System.Drawing.Point(960, 533);
            this.btnSetAllThresholds.Margin = new System.Windows.Forms.Padding(5);
            this.btnSetAllThresholds.Name = "btnSetAllThresholds";
            this.btnSetAllThresholds.Size = new System.Drawing.Size(200, 45);
            this.btnSetAllThresholds.TabIndex = 5;
            this.btnSetAllThresholds.Text = "批量设置门限";
            this.btnSetAllThresholds.UseVisualStyleBackColor = false;
            this.btnSetAllThresholds.Click += new System.EventHandler(this.btnSetAllThresholds_Click);
            // 
            // btnWriteThresholds
            // 
            this.btnWriteThresholds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnWriteThresholds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(39)))), ((int)(((byte)(176)))));
            this.btnWriteThresholds.FlatAppearance.BorderSize = 0;
            this.btnWriteThresholds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWriteThresholds.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnWriteThresholds.ForeColor = System.Drawing.Color.White;
            this.btnWriteThresholds.Location = new System.Drawing.Point(640, 533);
            this.btnWriteThresholds.Margin = new System.Windows.Forms.Padding(5);
            this.btnWriteThresholds.Name = "btnWriteThresholds";
            this.btnWriteThresholds.Size = new System.Drawing.Size(200, 45);
            this.btnWriteThresholds.TabIndex = 7;
            this.btnWriteThresholds.Text = "写入所有门限";
            this.btnWriteThresholds.UseVisualStyleBackColor = false;
            this.btnWriteThresholds.Click += new System.EventHandler(this.btnWriteThresholds_Click);
            // 
            // btnReadThresholds
            // 
            this.btnReadThresholds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReadThresholds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnReadThresholds.FlatAppearance.BorderSize = 0;
            this.btnReadThresholds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadThresholds.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnReadThresholds.ForeColor = System.Drawing.Color.White;
            this.btnReadThresholds.Location = new System.Drawing.Point(430, 533);
            this.btnReadThresholds.Margin = new System.Windows.Forms.Padding(5);
            this.btnReadThresholds.Name = "btnReadThresholds";
            this.btnReadThresholds.Size = new System.Drawing.Size(200, 45);
            this.btnReadThresholds.TabIndex = 6;
            this.btnReadThresholds.Text = "读取门限值";
            this.btnReadThresholds.UseVisualStyleBackColor = false;
            this.btnReadThresholds.Click += new System.EventHandler(this.btnReadThresholds_Click);
            // 
            // btnClearAllFlags
            // 
            this.btnClearAllFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearAllFlags.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnClearAllFlags.FlatAppearance.BorderSize = 0;
            this.btnClearAllFlags.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAllFlags.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearAllFlags.ForeColor = System.Drawing.Color.White;
            this.btnClearAllFlags.Location = new System.Drawing.Point(10, 533);
            this.btnClearAllFlags.Margin = new System.Windows.Forms.Padding(5);
            this.btnClearAllFlags.Name = "btnClearAllFlags";
            this.btnClearAllFlags.Size = new System.Drawing.Size(200, 45);
            this.btnClearAllFlags.TabIndex = 3;
            this.btnClearAllFlags.Text = "清除跌落标志";
            this.btnClearAllFlags.UseVisualStyleBackColor = false;
            this.btnClearAllFlags.Click += new System.EventHandler(this.btnClearAllFlags_Click);
            // 
            // dgvChannels
            // 
            this.dgvChannels.AllowUserToAddRows = false;
            this.dgvChannels.AllowUserToDeleteRows = false;
            this.dgvChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvChannels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChannels.Location = new System.Drawing.Point(10, 30);
            this.dgvChannels.Margin = new System.Windows.Forms.Padding(5);
            this.dgvChannels.Name = "dgvChannels";
            this.dgvChannels.RowHeadersVisible = false;
            this.dgvChannels.RowTemplate.Height = 23;
            this.dgvChannels.Size = new System.Drawing.Size(1080, 490);
            this.dgvChannels.TabIndex = 8;
            // 
            // ChannelMonitorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupBoxMain);
            this.Name = "ChannelMonitorView";
            this.Size = new System.Drawing.Size(1100, 600);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
