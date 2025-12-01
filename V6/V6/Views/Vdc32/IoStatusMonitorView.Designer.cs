using GJVdc32Tool.Controls;

namespace GJVdc32Tool.Views.Vdc32
{
    partial class IoStatusMonitorView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.TableLayoutPanel tableLayout;
        private StatusIndicator indS1Switch;
        private StatusIndicator indWaterSelf;
        private StatusIndicator indWaterPar;
        private StatusIndicator indJig;
        private StatusIndicator indContactor;
        private StatusIndicator indFan;
        private StatusIndicator indAcOnDep;
        private System.Windows.Forms.Label lblS1Switch;
        private System.Windows.Forms.Label lblWaterSelf;
        private System.Windows.Forms.Label lblWaterPar;
        private System.Windows.Forms.Label lblJig;
        private System.Windows.Forms.Label lblContactor;
        private System.Windows.Forms.Label lblFan;
        private System.Windows.Forms.Label lblAcOnDep;

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
            this.indS1Switch = new StatusIndicator();
            this.lblS1Switch = new System.Windows.Forms.Label();
            this.indWaterSelf = new StatusIndicator();
            this.lblWaterSelf = new System.Windows.Forms.Label();
            this.indWaterPar = new StatusIndicator();
            this.lblWaterPar = new System.Windows.Forms.Label();
            this.indJig = new StatusIndicator();
            this.lblJig = new System.Windows.Forms.Label();
            this.indContactor = new StatusIndicator();
            this.lblContactor = new System.Windows.Forms.Label();
            this.indFan = new StatusIndicator();
            this.lblFan = new System.Windows.Forms.Label();
            this.indAcOnDep = new StatusIndicator();
            this.lblAcOnDep = new System.Windows.Forms.Label();
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
            this.groupBoxMain.Size = new System.Drawing.Size(600, 320);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "IO 实时状态监测";
            // 
            // tableLayout
            // 
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout.Controls.Add(this.indS1Switch, 0, 0);
            this.tableLayout.Controls.Add(this.lblS1Switch, 1, 0);
            this.tableLayout.Controls.Add(this.indWaterSelf, 0, 1);
            this.tableLayout.Controls.Add(this.lblWaterSelf, 1, 1);
            this.tableLayout.Controls.Add(this.indWaterPar, 0, 2);
            this.tableLayout.Controls.Add(this.lblWaterPar, 1, 2);
            this.tableLayout.Controls.Add(this.indJig, 0, 3);
            this.tableLayout.Controls.Add(this.lblJig, 1, 3);
            this.tableLayout.Controls.Add(this.indContactor, 0, 4);
            this.tableLayout.Controls.Add(this.lblContactor, 1, 4);
            this.tableLayout.Controls.Add(this.indFan, 0, 5);
            this.tableLayout.Controls.Add(this.lblFan, 1, 5);
            this.tableLayout.Controls.Add(this.indAcOnDep, 0, 6);
            this.tableLayout.Controls.Add(this.lblAcOnDep, 1, 6);
            this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout.Location = new System.Drawing.Point(10, 33);
            this.tableLayout.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayout.Name = "tableLayout";
            this.tableLayout.RowCount = 7;
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayout.Size = new System.Drawing.Size(580, 277);
            this.tableLayout.TabIndex = 0;
            // 
            // indS1Switch
            // 
            this.indS1Switch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indS1Switch.Location = new System.Drawing.Point(3, 3);
            this.indS1Switch.Name = "indS1Switch";
            this.indS1Switch.Size = new System.Drawing.Size(84, 33);
            this.indS1Switch.State = StatusIndicator.IndicatorState.Off;
            this.indS1Switch.TabIndex = 0;
            // 
            // lblS1Switch
            // 
            this.lblS1Switch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblS1Switch.AutoSize = true;
            this.lblS1Switch.Location = new System.Drawing.Point(93, 12);
            this.lblS1Switch.Name = "lblS1Switch";
            this.lblS1Switch.Size = new System.Drawing.Size(82, 17);
            this.lblS1Switch.TabIndex = 1;
            this.lblS1Switch.Text = "S1切换开关";
            // 
            // indWaterSelf
            // 
            this.indWaterSelf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indWaterSelf.Location = new System.Drawing.Point(3, 42);
            this.indWaterSelf.Name = "indWaterSelf";
            this.indWaterSelf.Size = new System.Drawing.Size(84, 33);
            this.indWaterSelf.State = StatusIndicator.IndicatorState.Off;
            this.indWaterSelf.TabIndex = 2;
            // 
            // lblWaterSelf
            // 
            this.lblWaterSelf.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblWaterSelf.AutoSize = true;
            this.lblWaterSelf.Location = new System.Drawing.Point(93, 51);
            this.lblWaterSelf.Name = "lblWaterSelf";
            this.lblWaterSelf.Size = new System.Drawing.Size(90, 17);
            this.lblWaterSelf.TabIndex = 3;
            this.lblWaterSelf.Text = "防水检测（自）";
            // 
            // indWaterPar
            // 
            this.indWaterPar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indWaterPar.Location = new System.Drawing.Point(3, 81);
            this.indWaterPar.Name = "indWaterPar";
            this.indWaterPar.Size = new System.Drawing.Size(84, 33);
            this.indWaterPar.State = StatusIndicator.IndicatorState.Off;
            this.indWaterPar.TabIndex = 4;
            // 
            // lblWaterPar
            // 
            this.lblWaterPar.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblWaterPar.AutoSize = true;
            this.lblWaterPar.Location = new System.Drawing.Point(93, 90);
            this.lblWaterPar.Name = "lblWaterPar";
            this.lblWaterPar.Size = new System.Drawing.Size(90, 17);
            this.lblWaterPar.TabIndex = 5;
            this.lblWaterPar.Text = "防水检测（并）";
            // 
            // indJig
            // 
            this.indJig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indJig.Location = new System.Drawing.Point(3, 120);
            this.indJig.Name = "indJig";
            this.indJig.Size = new System.Drawing.Size(84, 33);
            this.indJig.State = StatusIndicator.IndicatorState.Off;
            this.indJig.TabIndex = 6;
            // 
            // lblJig
            // 
            this.lblJig.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblJig.AutoSize = true;
            this.lblJig.Location = new System.Drawing.Point(93, 129);
            this.lblJig.Name = "lblJig";
            this.lblJig.Size = new System.Drawing.Size(68, 17);
            this.lblJig.TabIndex = 7;
            this.lblJig.Text = "治具在位";
            // 
            // indContactor
            // 
            this.indContactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indContactor.Location = new System.Drawing.Point(3, 159);
            this.indContactor.Name = "indContactor";
            this.indContactor.Size = new System.Drawing.Size(84, 33);
            this.indContactor.State = StatusIndicator.IndicatorState.Off;
            this.indContactor.TabIndex = 8;
            // 
            // lblContactor
            // 
            this.lblContactor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblContactor.AutoSize = true;
            this.lblContactor.Location = new System.Drawing.Point(93, 168);
            this.lblContactor.Name = "lblContactor";
            this.lblContactor.Size = new System.Drawing.Size(92, 17);
            this.lblContactor.TabIndex = 9;
            this.lblContactor.Text = "接触器信号";
            // 
            // indFan
            // 
            this.indFan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indFan.Location = new System.Drawing.Point(3, 198);
            this.indFan.Name = "indFan";
            this.indFan.Size = new System.Drawing.Size(84, 33);
            this.indFan.State = StatusIndicator.IndicatorState.Off;
            this.indFan.TabIndex = 10;
            // 
            // lblFan
            // 
            this.lblFan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFan.AutoSize = true;
            this.lblFan.Location = new System.Drawing.Point(93, 207);
            this.lblFan.Name = "lblFan";
            this.lblFan.Size = new System.Drawing.Size(68, 17);
            this.lblFan.TabIndex = 11;
            this.lblFan.Text = "风扇状态";
            // 
            // indAcOnDep
            // 
            this.indAcOnDep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indAcOnDep.Location = new System.Drawing.Point(3, 237);
            this.indAcOnDep.Name = "indAcOnDep";
            this.indAcOnDep.Size = new System.Drawing.Size(84, 37);
            this.indAcOnDep.State = StatusIndicator.IndicatorState.Off;
            this.indAcOnDep.TabIndex = 12;
            // 
            // lblAcOnDep
            // 
            this.lblAcOnDep.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAcOnDep.AutoSize = true;
            this.lblAcOnDep.Location = new System.Drawing.Point(93, 249);
            this.lblAcOnDep.Name = "lblAcOnDep";
            this.lblAcOnDep.Size = new System.Drawing.Size(194, 17);
            this.lblAcOnDep.TabIndex = 13;
            this.lblAcOnDep.Text = "AC上电依赖治具在位（保护）";
            // 
            // IoStatusMonitorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.groupBoxMain);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "IoStatusMonitorView";
            this.Size = new System.Drawing.Size(600, 320);
            this.groupBoxMain.ResumeLayout(false);
            this.tableLayout.ResumeLayout(false);
            this.tableLayout.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
