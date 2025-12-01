using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Exporters
{
    /// <summary>
    /// 数据导出器
    /// 职责：将设备数据导出为各种格式
    /// </summary>
    public class DataExporter : IDataExporter
    {
        #region 常量定义

        private const string CSV_SEPARATOR = ",";
        private const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        #endregion

        #region 公共方法

        /// <summary>
        /// 导出 VDC-32 通道数据为 CSV
        /// </summary>
        public async Task<ExportResult> ExportVdc32ToCsvAsync(
            string filePath,
            double[] voltages,
            bool[] alarms,
            string deviceInfo = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return ExportResult.Fail("文件路径不能为空");

            if (voltages == null || voltages.Length == 0)
                return ExportResult.Fail("没有数据可导出");

            try
            {
                var sb = new StringBuilder();

                // 添加头部信息
                sb.AppendLine($"# VDC-32 电压数据导出");
                sb.AppendLine($"# 导出时间: {DateTime.Now.ToString(DATETIME_FORMAT)}");
                if (!string.IsNullOrEmpty(deviceInfo))
                {
                    sb.AppendLine($"# 设备信息: {deviceInfo}");
                }
                sb.AppendLine();

                // 表头
                sb.AppendLine("通道,电压(V),状态");

                // 数据行
                for (int i = 0; i < voltages.Length; i++)
                {
                    string status = (alarms != null && i < alarms.Length && alarms[i])
                        ? "报警" : "正常";
                    sb.AppendLine($"CH{i + 1:D2},{voltages[i]:F3},{status}");
                }

                await WriteFileAsync(filePath, sb.ToString());

                return ExportResult.Ok(filePath, voltages.Length);
            }
            catch (Exception ex)
            {
                return ExportResult.Fail($"导出失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 导出负载设备通道数据为 CSV
        /// </summary>
        public async Task<ExportResult> ExportLoadDeviceToCsvAsync(
            string filePath,
            LoadChannelExportData[] channels,
            string deviceInfo = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return ExportResult.Fail("文件路径不能为空");

            if (channels == null || channels.Length == 0)
                return ExportResult.Fail("没有数据可导出");

            try
            {
                var sb = new StringBuilder();

                // 添加头部信息
                sb.AppendLine($"# GJDD-750 负载数据导出");
                sb.AppendLine($"# 导出时间: {DateTime.Now.ToString(DATETIME_FORMAT)}");
                if (!string.IsNullOrEmpty(deviceInfo))
                {
                    sb.AppendLine($"# 设备信息: {deviceInfo}");
                }
                sb.AppendLine();

                // 表头
                sb.AppendLine("通道,电流(A),电压(V),功率(W),状态");

                // 数据行
                foreach (var ch in channels)
                {
                    string status = ch.IsOn ? "开启" : "关闭";
                    if (ch.HasFault) status = "故障";

                    sb.AppendLine($"CH{ch.ChannelIndex + 1},{ch.Current:F2},{ch.Voltage:F2},{ch.Power:F1},{status}");
                }

                await WriteFileAsync(filePath, sb.ToString());

                return ExportResult.Ok(filePath, channels.Length);
            }
            catch (Exception ex)
            {
                return ExportResult.Fail($"导出失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 导出日志为文本文件
        /// </summary>
        public async Task<ExportResult> ExportLogAsync(string filePath, string logContent)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return ExportResult.Fail("文件路径不能为空");

            if (string.IsNullOrEmpty(logContent))
                return ExportResult.Fail("日志内容为空");

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"# GJVdc32Tool 运行日志");
                sb.AppendLine($"# 导出时间: {DateTime.Now.ToString(DATETIME_FORMAT)}");
                sb.AppendLine(new string('-', 50));
                sb.AppendLine();
                sb.Append(logContent);

                await WriteFileAsync(filePath, sb.ToString());

                int lineCount = logContent.Split('\n').Length;
                return ExportResult.Ok(filePath, lineCount);
            }
            catch (Exception ex)
            {
                return ExportResult.Fail($"导出失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成默认导出文件名
        /// </summary>
        public string GenerateFileName(string prefix, ExportFormat format)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string extension = GetFileExtension(format);
            return $"{prefix}_{timestamp}.{extension}";
        }

        #endregion

        #region 私有方法

        private async Task WriteFileAsync(string filePath, string content)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                await writer.WriteAsync(content);
            }
        }

        private string GetFileExtension(ExportFormat format)
        {
            switch (format)
            {
                case ExportFormat.Csv: return "csv";
                case ExportFormat.Excel: return "xlsx";
                case ExportFormat.Json: return "json";
                case ExportFormat.Text: return "txt";
                default: return "txt";
            }
        }

        #endregion
    }

    /// <summary>
    /// 负载通道导出数据
    /// </summary>
    public class LoadChannelExportData
    {
        public int ChannelIndex { get; set; }
        public double Current { get; set; }
        public double Voltage { get; set; }
        public double Power { get; set; }
        public bool IsOn { get; set; }
        public bool HasFault { get; set; }
    }
}
