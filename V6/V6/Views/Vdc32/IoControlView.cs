using System;
using System.Windows.Forms;
using GJVdc32Tool.Controls;
using GJVdc32Tool.Models;

namespace GJVdc32Tool.Views.Vdc32
{
    /// <summary>
    /// IO 输出控制视图（4个 ToggleSwitch）
    /// </summary>
    public partial class IoControlView : UserControl
    {
        public event EventHandler<IoControlEventArgs> IoControlChanged;

        public IoControlView()
        {
            InitializeComponent();
            AttachEvents();
        }

        public void UpdateOutputStatus(IoStatus status)
        {
            if (status == null)
            {
                return;
            }

            SetToggleWithoutEvent(togglePtc, status.Io0OutputLow);
            SetToggleWithoutEvent(toggleAc, status.Io1OutputLow);
            SetToggleWithoutEvent(togglePson, status.Io2OutputLow);
            SetToggleWithoutEvent(toggleFan, status.Io3OutputLow);
        }

        public void SetEnabled(bool enabled)
        {
            togglePtc.Enabled = enabled;
            toggleAc.Enabled = enabled;
            togglePson.Enabled = enabled;
            toggleFan.Enabled = enabled;
        }

        private void AttachEvents()
        {
            togglePtc.CheckedChanged += OnToggleChanged;
            toggleAc.CheckedChanged += OnToggleChanged;
            togglePson.CheckedChanged += OnToggleChanged;
            toggleFan.CheckedChanged += OnToggleChanged;
        }

        private void SetToggleWithoutEvent(ToggleSwitch toggle, bool isChecked)
        {
            toggle.CheckedChanged -= OnToggleChanged;
            toggle.Checked = isChecked;
            toggle.CheckedChanged += OnToggleChanged;
        }

        private void OnToggleChanged(object sender, EventArgs e)
        {
            var toggle = (ToggleSwitch)sender;
            var command = GetCommandForToggle(toggle);

            IoControlChanged?.Invoke(this, new IoControlEventArgs
            {
                Command = command,
                IsOn = toggle.Checked,
                ToggleName = GetToggleName(toggle)
            });
        }

        private IoCommand GetCommandForToggle(ToggleSwitch toggle)
        {
            if (toggle == togglePtc)
            {
                return toggle.Checked ? IoCommand.PtcOn : IoCommand.PtcOff;
            }

            if (toggle == toggleAc)
            {
                return toggle.Checked ? IoCommand.AcOn : IoCommand.AcOff;
            }

            if (toggle == togglePson)
            {
                return toggle.Checked ? IoCommand.PsonOn : IoCommand.PsonOff;
            }

            if (toggle == toggleFan)
            {
                return toggle.Checked ? IoCommand.FanOn : IoCommand.FanOff;
            }

            return IoCommand.PtcOff;
        }

        private string GetToggleName(ToggleSwitch toggle)
        {
            if (toggle == togglePtc)
            {
                return "PTC加热器";
            }

            if (toggle == toggleAc)
            {
                return "AC电源";
            }

            if (toggle == togglePson)
            {
                return "PSON";
            }

            if (toggle == toggleFan)
            {
                return "风扇";
            }

            return "未知";
        }
    }

    public class IoControlEventArgs : EventArgs
    {
        public IoCommand Command { get; set; }
        public bool IsOn { get; set; }
        public string ToggleName { get; set; }
    }
}
