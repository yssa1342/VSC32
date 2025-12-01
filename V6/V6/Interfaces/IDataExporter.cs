using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// 导出结果
    /// </summary>
    public class ExportResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// 导出的文件路径
        /// </summary>
        public string FilePath { get; set; }
        
        /// <summary>
        /// 导出的记录数
        /// </summary>
        public int RecordCount { get; set; }
        
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 数据导出器接口
    /// </summary>
    public interface IDataExporter
    {
        /// <summary>
        /// 支持的文件扩展名
        /// </summary>
        string FileExtension { get; }
        
        /// <summary>
        /// 文件类型过滤器 (用于 SaveFileDialog)
        /// </summary>
        string FileFilter { get; }
        
        /// <summary>
        /// 导出 VDC-32 通道数据
        /// </summary>
        /// <typeparam name="T">通道数据类型</typeparam>
        /// <param name="data">通道数据列表</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>导出结果</returns>
        Task<ExportResult> ExportVdc32DataAsync<T>(IEnumerable<T> data, string filePath) where T : class;
        
        /// <summary>
        /// 导出负载设备实时数据
        /// </summary>
        /// <typeparam name="T">实时数据类型</typeparam>
        /// <param name="data">实时数据</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>导出结果</returns>
        Task<ExportResult> ExportLoadDeviceDataAsync<T>(T data, string filePath) where T : class;
        
        /// <summary>
        /// 导出历史数据
        /// </summary>
        /// <typeparam name="T">快照数据类型</typeparam>
        /// <param name="snapshots">历史快照列表</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>导出结果</returns>
        Task<ExportResult> ExportHistoryDataAsync<T>(IEnumerable<T> snapshots, string filePath) where T : class;
        
        /// <summary>
        /// 生成默认文件名
        /// </summary>
        /// <param name="prefix">文件名前缀</param>
        /// <returns>带时间戳的文件名</returns>
        string GenerateDefaultFileName(string prefix);
    }
}
