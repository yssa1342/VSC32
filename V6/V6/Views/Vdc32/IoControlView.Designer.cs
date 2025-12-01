using GJVdc32Tool.Controls;

namespace GJVdc32Tool.Views.Vdc32
{
    partial class IoControlView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.TableLayoutPanel tableLayout;
        private System.Windows.Forms.Label lblPtc;
        private System.Windows.Forms.Label lblAc;
        private System.Windows.Forms.Label lblPson;
        private System.Windows.Forms.Label lblFan;
        private ToggleSwitch togglePtc;
        private ToggleSwitch toggleAc;
        private ToggleSwitch togglePson;
        private ToggleSwitch toggleFan;

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
            this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblPtc = new System.Windows.Forms.Label();
            this.togglePtc = new ToggleSwitch();
            this.lblAc = new System.Windows.Forms.Label();
            this.toggleAc = new ToggleSwitch();
            this.lblPson = new System.Windows.Forms.Label();
            this.togglePson = new ToggleSwitch();
            this.lblFan = new System.Windows.Forms.Label();
            this.toggleFan = new ToggleSwitch();
            this.groupBoxMain.SuspendLayout();
            this.tableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.tableLayout);
            this.groupBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMain.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Padding = new System.Windows.Forms.Padding(10, 15, 10, 10);
            this.groupBoxMain.Size = new System.Drawing.Size(600, 220);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "IO 输出控制";
            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Controls.Add(this.lblPtc, 0, 0);
            this.tableLayout.Controls.Add(this.togglePtc, 1, 0);
            this.tableLayout.Controls.Add(this.lblAc, 0, 1);
            this.tableLayout.Controls.Add(this.toggleAc, 1, 1);
            this.tableLayout.Controls.Add(this.lblPson, 0, 2);
            this.tableLayout.Controls.Add(this.togglePson, 1, 2);
            this.tableLayout.Controls.Add(this.lblFan, 0, 3);
            this.tableLayout.Controls.Add(this.toggleFan, 1, 3);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(10, 33);
            this.tableLayout.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 4;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout.Size = new System.Drawing.Size(580, 177);
            this.tableLayout.TabIndex = 0;
            // 
            // lblPtc
            // 
            this.lblPtc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPtc.AutoSize = true;
            this.lblPtc.Location = new System.Drawing.Point(3, 13);
            this.lblPtc.Name = "lblPtc";
            this.lblPtc.Size = new System.Drawing.Size(92, 17);
            this.lblPtc.TabIndex = 0;
            this.lblPtc.Text = "PTC加热器";
            // 
            // togglePtc
            // 
            this.togglePtc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.togglePtc.Location = new System.Drawing.Point(123, 9);
            this.togglePtc.MinimumSize = new System.Drawing.Size(60, 30);
            this.togglePtc.Name = "togglePtc";
            this.togglePtc.Size = new System.Drawing.Size(80, 30);
            this.togglePtc.TabIndex = 1;
            // 
            // lblAc
            // 
            this.lblAc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAc.AutoSize = true;
            this.lblAc.Location = new System.Drawing.Point(3, 57);
            this.lblAc.Name = "lblAc";
            this.lblAc.Size = new System.Drawing.Size(60, 17);
            this.lblAc.TabIndex = 2;
            this.lblAc.Text = "AC电源";
            // 
            // toggleAc
            // 
            this.toggleAc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.toggleAc.Location = new System.Drawing.Point(123, 53);
            this.toggleAc.MinimumSize = new System.Drawing.Size(60, 30);
            this.toggleAc.Name = "toggleAc";
            this.toggleAc.Size = new System.Drawing.Size(80, 30);
            this.toggleAc.TabIndex = 3;
            // 
            // lblPson
            // 
            this.lblPson.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPson.AutoSize = true;
            this.lblPson.Location = new System.Drawing.Point(3, 101);
            this.lblPson.Name = "lblPson";
            this.lblPson.Size = new System.Drawing.Size(42, 17);
            this.lblPson.TabIndex = 4;
            this.lblPson.Text = "PSON";
            // 
            // togglePson
            // 
            this.togglePson.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.togglePson.Location = new System.Drawing.Point(123, 97);
            this.togglePson.MinimumSize = new System.Drawing.Size(60, 30);
            this.togglePson.Name = "togglePson";
            this.togglePson.Size = new System.Drawing.Size(80, 30);
            this.togglePson.TabIndex = 5;
            // 
            // lblFan
            // 
            this.lblFan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFan.AutoSize = true;
            this.lblFan.Location = new System.Drawing.Point(3, 145);
            this.lblFan.Name = "lblFan";
            this.lblFan.Size = new System.Drawing.Size(36, 17);
            this.lblFan.TabIndex = 6;
            this.lblFan.Text = "风扇";
            // 
            // toggleFan
            // 
            this.toggleFan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.toggleFan.Location = new System.Drawing.Point(123, 141);
            this.toggleFan.MinimumSize = new System.Drawing.Size(60, 30);
            this.toggleFan.Name = "toggleFan";
            this.toggleFan.Size = new System.Drawing.Size(80, 30);
            this.toggleFan.TabIndex = 7;
            // 
            // IoControlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupBoxMain);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "IoControlView";
            this.Size = new System.Drawing.Size(600, 220);
            this.groupBoxMain.ResumeLayout(false);
            this.tableLayout.ResumeLayout(false);
            this.tableLayout.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
