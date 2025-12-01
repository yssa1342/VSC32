using System.Windows.Forms;
using GJVdc32Tool.Controls;
using GJVdc32Tool.Models;

namespace GJVdc32Tool.Views.Vdc32
{
    /// <summary>
    /// IO 实时状态监测视图（7个状态指示灯）
    /// </summary>
    public partial class IoStatusMonitorView : UserControl
    {
        public IoStatusMonitorView()
        {
            InitializeComponent();
        }

        public void UpdateStatus(IoStatus status)
        {
            if (status == null)
            {
                return;
            }

            UpdateIndicator(indS1Switch, status.S1Switch, false);
            UpdateIndicator(indWaterSelf, status.WaterLeakSelf, true);
            UpdateIndicator(indWaterPar, status.WaterLeakParallel, true);
            UpdateIndicator(indJig, status.JigInPlace, false);
            UpdateIndicator(indContactor, status.ContactorSignal, false);
            UpdateIndicator(indFan, status.FanStatus, false);
            UpdateIndicator(indAcOnDep, status.AcOnDependsOnJig, false);
        }

        public void ResetAll()
        {
            indS1Switch.State = StatusIndicator.IndicatorState.Off;
            indWaterSelf.State = StatusIndicator.IndicatorState.Off;
            indWaterPar.State = StatusIndicator.IndicatorState.Off;
            indJig.State = StatusIndicator.IndicatorState.Off;
            indContactor.State = StatusIndicator.IndicatorState.Off;
            indFan.State = StatusIndicator.IndicatorState.Off;
            indAcOnDep.State = StatusIndicator.IndicatorState.Off;
        }

        private void UpdateIndicator(StatusIndicator indicator, bool isActive, bool isErrorWhenActive)
        {
            if (isActive)
            {
                indicator.State = isErrorWhenActive
                    ? StatusIndicator.IndicatorState.Error
                    : StatusIndicator.IndicatorState.On;
            }
            else
            {
                indicator.State = StatusIndicator.IndicatorState.Off;
            }
        }
    }
}
