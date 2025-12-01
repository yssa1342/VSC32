using System;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// 通道电流设置事件参数
    /// </summary>
    public class ChannelCurrentEventArgs : EventArgs
    {
        /// <summary>
        /// 通道索引 (0-7)
        /// </summary>
        public int ChannelIndex { get; set; }

        /// <summary>
        /// 电流值 (A)
        /// </summary>
        public double Current { get; set; }

        public ChannelCurrentEventArgs(int channelIndex, double current)
        {
            ChannelIndex = channelIndex;
            Current = current;
        }
    }

    /// <summary>
    /// 通道配置事件参数
    /// </summary>
    public class ChannelConfigEventArgs : EventArgs
    {
        /// <summary>
        /// 通道索引 (0-7)
        /// </summary>
        public int ChannelIndex { get; set; }

        /// <summary>
        /// 电流值 (A)
        /// </summary>
        public double Current { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOn { get; set; }

        public ChannelConfigEventArgs(int channelIndex, double current, bool isOn)
        {
            ChannelIndex = channelIndex;
            Current = current;
            IsOn = isOn;
        }
    }

    /// <summary>
    /// 导出请求事件参数
    /// </summary>
    public class ExportRequestEventArgs : EventArgs
    {
        /// <summary>
        /// 导出格式
        /// </summary>
        public ExportFormat Format { get; set; }

        /// <summary>
        /// 文件路径（可选）
        /// </summary>
        public string FilePath { get; set; }

        public ExportRequestEventArgs(ExportFormat format, string filePath = null)
        {
            Format = format;
            FilePath = filePath;
        }
    }

    /// <summary>
    /// 导出格式
    /// </summary>
    public enum ExportFormat
    {
        /// <summary>
        /// CSV 格式
        /// </summary>
        Csv,

        /// <summary>
        /// Excel 格式
        /// </summary>
        Excel,

        /// <summary>
        /// JSON 格式
        /// </summary>
        Json,

        /// <summary>
        /// 文本格式
        /// </summary>
        Text
    }
}
