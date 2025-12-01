using System;
using System.Drawing;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// GJDD-750 负载设备视图接口
    /// 定义负载设备视图层需要实现的功能
    /// </summary>
    public interface ILoadDeviceView
    {
        #region 设备状态显示
        
        /// <summary>
        /// 设备状态文本
        /// </summary>
        string DeviceStatusText { set; }
        
        /// <summary>
        /// 设备状态背景色
        /// </summary>
        Color DeviceStatusColor { set; }
        
        /// <summary>
        /// 是否为 800V 设备
        /// </summary>
        bool Is800VDevice { get; set; }
        
        /// <summary>
        /// 电源设置 Tab 是否启用
        /// </summary>
        bool PowerTabEnabled { set; }
        
        #endregion

        #region 实时数据显示
        
        /// <summary>
        /// 更新通道电压
        /// </summary>
        void UpdateChannelVoltage(int channelIndex, double voltage, Color color);
        
        /// <summary>
        /// 更新通道电流
        /// </summary>
        void UpdateChannelCurrent(int channelIndex, double current, Color color);
        
        /// <summary>
        /// 更新通道功率
        /// </summary>
        void UpdateChannelPower(int channelIndex, double power, Color color);
        
        /// <summary>
        /// 更新通道输入电压
        /// </summary>
        void UpdateChannelInputVoltage(int channelIndex, double voltage, Color color);
        
        /// <summary>
        /// 更新通道 DC-DC 状态
        /// </summary>
        void UpdateChannelDcDcStatus(int channelIndex, bool isOn, Color color);
        
        /// <summary>
        /// 更新通道状态标志
        /// </summary>
        void UpdateChannelStatusFlag(int channelIndex, string status, Color color, Color bgColor);
        
        /// <summary>
        /// 更新通道工作模式
        /// </summary>
        void UpdateChannelWorkMode(int channelIndex, string mode, Color color);
        
        /// <summary>
        /// 更新通道 Von 点
        /// </summary>
        void UpdateChannelVonPoint(int channelIndex, double von, Color color);
        
        /// <summary>
        /// 更新通道设定值
        /// </summary>
        void UpdateChannelLoadValue(int channelIndex, string value, Color color);
        
        /// <summary>
        /// 更新通道延迟
        /// </summary>
        void UpdateChannelDelay(int channelIndex, string delay, Color color);
        
        #endregion

        #region 通道监控卡片显示
        
        /// <summary>
        /// 更新通道卡片状态
        /// </summary>
        /// <param name="channelIndex">通道索引 (0-7)</param>
        /// <param name="isOnline">是否在线</param>
        /// <param name="hasAlarm">是否有告警</param>
        /// <param name="voltage">电压</param>
        /// <param name="current">电流</param>
        /// <param name="power">功率</param>
        /// <param name="statusText">状态文本</param>
        void UpdateChannelCard(int channelIndex, bool isOnline, bool hasAlarm, 
            double voltage, double current, double power, string statusText);
        
        /// <summary>
        /// 更新监控摘要
        /// </summary>
        /// <param name="onlineCount">在线数量</param>
        /// <param name="totalPower">总功率</param>
        /// <param name="alarmCount">告警数量</param>
        void UpdateMonitorSummary(int onlineCount, double totalPower, int alarmCount);
        
        #endregion

        #region 逆变状态显示
        
        /// <summary>
        /// 更新逆变器整体状态摘要
        /// </summary>
        void UpdateInverterSummary(string statusText, bool hasFault);
        
        /// <summary>
        /// 更新逆变过温状态
        /// </summary>
        void UpdateInverterOverTemp(bool isOverTemp);
        
        /// <summary>
        /// 更新 AD 采样故障状态
        /// </summary>
        void UpdateInverterAdFault(bool isFault);
        
        /// <summary>
        /// 更新输出电压状态
        /// </summary>
        /// <param name="status">0=欠压, 1=正常, 2=过压</param>
        void UpdateInverterOutputVoltage(int status);
        
        /// <summary>
        /// 更新风扇状态
        /// </summary>
        void UpdateInverterFan(bool isFault);
        
        /// <summary>
        /// 更新超时故障状态
        /// </summary>
        void UpdateInverterTimeout(bool isTimeout);
        
        /// <summary>
        /// 更新直流母线电压状态
        /// </summary>
        /// <param name="status">0=欠压, 1=正常, 2=过压</param>
        void UpdateInverterDcBus(int status);
        
        #endregion

        #region 配置面板
        
        /// <summary>
        /// 单通道配置面板是否启用
        /// </summary>
        bool SingleChannelConfigEnabled { set; }
        
        /// <summary>
        /// 批量配置面板是否启用
        /// </summary>
        bool BatchConfigEnabled { set; }
        
        /// <summary>
        /// 获取当前选中的通道索引 (1-8)
        /// </summary>
        int SelectedChannel { get; }
        
        /// <summary>
        /// 获取当前选中的工作模式索引
        /// </summary>
        int SelectedMode { get; }
        
        /// <summary>
        /// 获取 Von 电压值
        /// </summary>
        double VonVoltage { get; set; }
        
        /// <summary>
        /// 获取负载值
        /// </summary>
        double LoadValue { get; set; }
        
        /// <summary>
        /// 获取延迟值
        /// </summary>
        double DelayValue { get; set; }
        
        #endregion

        #region 事件
        
        /// <summary>
        /// 800V 设备类型切换事件
        /// </summary>
        event EventHandler<bool> Device800VChanged;
        
        /// <summary>
        /// 请求读取单通道配置
        /// </summary>
        event EventHandler ReadSingleConfigRequested;
        
        /// <summary>
        /// 请求临时设置单通道
        /// </summary>
        event EventHandler TempSetSingleChannelRequested;
        
        /// <summary>
        /// 请求保存单通道到 EEPROM
        /// </summary>
        event EventHandler SaveSingleChannelRequested;
        
        /// <summary>
        /// 请求批量临时设置
        /// </summary>
        event EventHandler BatchTempSetRequested;
        
        /// <summary>
        /// 请求批量保存到 EEPROM
        /// </summary>
        event EventHandler BatchSaveRequested;
        
        /// <summary>
        /// 请求导出 CSV
        /// </summary>
        event EventHandler ExportCsvRequested;
        
        /// <summary>
        /// 请求设置电源电压
        /// </summary>
        event EventHandler<int> SetPowerVoltageRequested;
        
        /// <summary>
        /// 工作模式变更事件
        /// </summary>
        event EventHandler<int> ModeChanged;
        
        #endregion
    }
}
