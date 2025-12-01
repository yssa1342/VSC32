using System;
using System.Drawing;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// VDC-32 设备视图接口
    /// 定义 VDC-32 视图层需要实现的功能
    /// </summary>
    public interface IVdc32View
    {
        #region 设备信息显示
        
        /// <summary>
        /// 显示固件版本
        /// </summary>
        string FirmwareVersion { set; }
        
        /// <summary>
        /// 显示设备名称
        /// </summary>
        string DeviceName { set; }
        
        /// <summary>
        /// 显示从机地址
        /// </summary>
        string SlaveAddress { set; }
        
        #endregion

        #region 通道数据显示
        
        /// <summary>
        /// 更新单个通道的电压显示
        /// </summary>
        /// <param name="channelIndex">通道索引 (0-31)</param>
        /// <param name="voltage">电压值</param>
        /// <param name="color">显示颜色</param>
        void UpdateChannelVoltage(int channelIndex, double voltage, Color color);
        
        /// <summary>
        /// 更新单个通道的状态
        /// </summary>
        /// <param name="channelIndex">通道索引 (0-31)</param>
        /// <param name="status">状态枚举值</param>
        void UpdateChannelStatus(int channelIndex, int status);
        
        /// <summary>
        /// 批量更新所有通道数据
        /// </summary>
        void RefreshAllChannels();
        
        #endregion

        #region IO 状态显示
        
        /// <summary>
        /// 更新 S1 拨码开关状态
        /// </summary>
        bool S1SwitchStatus { set; }
        
        /// <summary>
        /// 更新自身漏水检测状态
        /// </summary>
        bool WaterLeakSelfStatus { set; }
        
        /// <summary>
        /// 更新并联漏水检测状态
        /// </summary>
        bool WaterLeakParallelStatus { set; }
        
        /// <summary>
        /// 更新治具到位状态
        /// </summary>
        bool JigInPlaceStatus { set; }
        
        /// <summary>
        /// 更新接触器信号状态
        /// </summary>
        bool ContactorStatus { set; }
        
        /// <summary>
        /// 更新风扇状态
        /// </summary>
        bool FanStatus { set; }
        
        /// <summary>
        /// 更新 AC 依赖治具状态
        /// </summary>
        bool AcDependsOnJigStatus { set; }
        
        #endregion

        #region IO 输出控制状态
        
        /// <summary>
        /// PTC 加热器开关状态
        /// </summary>
        bool PtcToggleState { get; set; }
        
        /// <summary>
        /// AC 安全联锁开关状态
        /// </summary>
        bool AcToggleState { get; set; }
        
        /// <summary>
        /// PSON 电源控制开关状态
        /// </summary>
        bool PsonToggleState { get; set; }
        
        /// <summary>
        /// 风扇开关状态
        /// </summary>
        bool FanToggleState { get; set; }
        
        #endregion

        #region 环境数据显示
        
        /// <summary>
        /// 显示设定温度
        /// </summary>
        int SetTemperature { get; set; }
        
        /// <summary>
        /// 显示当前温度
        /// </summary>
        string CurrentTemperature { set; }
        
        /// <summary>
        /// 显示风扇电流
        /// </summary>
        string FanCurrent { set; }
        
        #endregion

        #region 配置显示
        
        /// <summary>
        /// SN 序列号
        /// </summary>
        string SerialNumber { get; set; }
        
        /// <summary>
        /// SN 是否可编辑
        /// </summary>
        bool SerialNumberEditable { set; }
        
        /// <summary>
        /// SN 写入按钮是否可用
        /// </summary>
        bool SerialNumberWriteEnabled { set; }
        
        #endregion

        #region 事件
        
        /// <summary>
        /// PTC 开关切换事件
        /// </summary>
        event EventHandler<bool> PtcToggleChanged;
        
        /// <summary>
        /// AC 开关切换事件
        /// </summary>
        event EventHandler<bool> AcToggleChanged;
        
        /// <summary>
        /// PSON 开关切换事件
        /// </summary>
        event EventHandler<bool> PsonToggleChanged;
        
        /// <summary>
        /// 风扇开关切换事件
        /// </summary>
        event EventHandler<bool> FanToggleChanged;
        
        /// <summary>
        /// 请求导出 CSV 事件
        /// </summary>
        event EventHandler ExportCsvRequested;
        
        /// <summary>
        /// 请求设置所有阈值事件
        /// </summary>
        event EventHandler<double> SetAllThresholdsRequested;
        
        /// <summary>
        /// 请求清除所有标志事件
        /// </summary>
        event EventHandler ClearAllFlagsRequested;
        
        #endregion
    }
}
