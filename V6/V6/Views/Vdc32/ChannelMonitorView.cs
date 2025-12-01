using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GJVdc32Tool.Views.Vdc32
{
    /// <summary>
    /// 32 通道电压监测视图
    /// </summary>
    public partial class ChannelMonitorView : UserControl
    {
        public event EventHandler ExportRequested;
        public event EventHandler ClearFlagsRequested;
        public event EventHandler ReadThresholdsRequested;
        public event EventHandler WriteThresholdsRequested;
        public event EventHandler<double> SetAllThresholdsRequested;

        private BindingList<ChannelData> _channelDataList;

        public ChannelMonitorView()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        #region 公共方法

        public void UpdateChannelData(IList<ChannelData> data)
        {
            if (data == null || data.Count == 0)
                return;

            for (int i = 0; i < Math.Min(32, data.Count); i++)
            {
                _channelDataList[i].Voltage = data[i].Voltage;
                _channelDataList[i].Status = data[i].Status;
                _channelDataList[i].Threshold = data[i].Threshold;
                _channelDataList[i].RecoveryTime = data[i].RecoveryTime;
            }
        }

        public IList<ChannelData> GetChannelData()
        {
            return _channelDataList.ToList();
        }

        public void SetEnabled(bool enabled)
        {
            btnExportCsv.Enabled = true;
            btnClearAllFlags.Enabled = enabled;
            btnReadThresholds.Enabled = enabled;
            btnWriteThresholds.Enabled = enabled;
            btnSetAllThresholds.Enabled = enabled;
            txtSetAllThresholds.Enabled = enabled;
        }

        #endregion

        #region 私有方法

        private void InitializeDataGridView()
        {
            _channelDataList = new BindingList<ChannelData>();

            for (int i = 1; i <= 32; i++)
            {
                var channelData = new ChannelData { Channel = i };
                channelData.PropertyChanged += ChannelData_PropertyChanged;
                _channelDataList.Add(channelData);
            }

            ConfigureDataGridViewColumns();
            PopulateDataGridView();
        }

        private void ConfigureDataGridViewColumns()
        {
            dgvChannels.AutoGenerateColumns = false;
            dgvChannels.Columns.Clear();
            dgvChannels.RowHeadersVisible = false;
            dgvChannels.AllowUserToAddRows = false;
            dgvChannels.AllowUserToDeleteRows = false;
            dgvChannels.ReadOnly = true;

            for (int group = 1; group <= 4; group++)
            {
                dgvChannels.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = $"Ch{group}",
                    HeaderText = "通道",
                    Width = 80,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                });

                dgvChannels.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = $"V{group}",
                    HeaderText = "电压(V)",
                    Width = 120,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                });
            }
        }

        private void PopulateDataGridView()
        {
            dgvChannels.Rows.Clear();
            for (int row = 0; row < 8; row++)
            {
                dgvChannels.Rows.Add(
                    $"CH{row + 1}", "N/A",
                    $"CH{row + 9}", "N/A",
                    $"CH{row + 17}", "N/A",
                    $"CH{row + 25}", "N/A"
                );
            }
        }

        private void ChannelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ChannelData.Voltage) ||
                e.PropertyName == nameof(ChannelData.Status))
            {
                UpdateSingleChannelDisplay((ChannelData)sender);
            }
        }

        private void UpdateSingleChannelDisplay(ChannelData channelData)
        {
            int channelIndex = channelData.Channel - 1;
            int row = channelIndex % 8;
            int group = channelIndex / 8;

            string columnName = $"V{group + 1}";
            if (row < dgvChannels.Rows.Count)
            {
                dgvChannels.Rows[row].Cells[columnName].Value = channelData.VoltageText;
                dgvChannels.Rows[row].Cells[columnName].Style.ForeColor =
                    channelData.Voltage > 0 ? channelData.StatusColor : Color.Black;
            }
        }

        #endregion

        #region 事件处理

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            ExportRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnClearAllFlags_Click(object sender, EventArgs e)
        {
            ClearFlagsRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnReadThresholds_Click(object sender, EventArgs e)
        {
            ReadThresholdsRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnWriteThresholds_Click(object sender, EventArgs e)
        {
            WriteThresholdsRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnSetAllThresholds_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtSetAllThresholds.Text, out double threshold))
            {
                SetAllThresholdsRequested?.Invoke(this, threshold);
            }
        }

        #endregion
    }

    public class ChannelData : INotifyPropertyChanged
    {
        private double _voltage;
        private string _status;
        private double _threshold;
        private double _recoveryTime;

        public int Channel { get; set; }

        public double Voltage
        {
            get => _voltage;
            set
            {
                if (Math.Abs(_voltage - value) > double.Epsilon)
                {
                    _voltage = value;
                    OnPropertyChanged(nameof(Voltage));
                }
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public double Threshold
        {
            get => _threshold;
            set
            {
                if (Math.Abs(_threshold - value) > double.Epsilon)
                {
                    _threshold = value;
                    OnPropertyChanged(nameof(Threshold));
                }
            }
        }

        public double RecoveryTime
        {
            get => _recoveryTime;
            set
            {
                if (Math.Abs(_recoveryTime - value) > double.Epsilon)
                {
                    _recoveryTime = value;
                    OnPropertyChanged(nameof(RecoveryTime));
                }
            }
        }

        public string VoltageText => Voltage.ToString("F3");

        public Color StatusColor => Status switch
        {
            "正常" => Color.FromArgb(76, 175, 80),
            "报警" => Color.FromArgb(244, 67, 54),
            _ => Color.Black
        };

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
