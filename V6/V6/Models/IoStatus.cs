namespace GJVdc32Tool.Models
{
    /// <summary>
    /// IO 输入状态数据模型
    /// </summary>
    public class IoStatus
    {
        public bool S1Switch { get; set; }
        public bool WaterLeakSelf { get; set; }
        public bool WaterLeakParallel { get; set; }
        public bool JigInPlace { get; set; }
        public bool ContactorSignal { get; set; }
        public bool FanStatus { get; set; }
        public bool AcOnDependsOnJig { get; set; }
        public bool Io0OutputLow { get; set; }
        public bool Io1OutputLow { get; set; }
        public bool Io2OutputLow { get; set; }
        public bool Io3OutputLow { get; set; }
    }
}
