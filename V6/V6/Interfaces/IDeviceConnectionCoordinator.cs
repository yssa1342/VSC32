using System;
using System.Threading.Tasks;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// 设备连接类型枚举
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// 无设备连接
        /// </summary>
        None,
        
        /// <summary>
        /// VDC-32 检测板
        /// </summary>
        VDC32,
        
        /// <summary>
        /// GJDD-750 负载设备
        /// </summary>
        LoadDevice
    }

    /// <summary>
    /// 连接状态变更事件参数
    /// </summary>
    public class ConnectionStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        public DeviceType DeviceType { get; set; }
        
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected { get; set; }
        
        /// <summary>
        /// 连接信息描述
        /// </summary>
        public string ConnectionInfo { get; set; }
    }

    /// <summary>
    /// 设备连接协调器接口
    /// 负责管理多设备连接的互斥和协调
    /// </summary>
    public interface IDeviceConnectionCoordinator
    {
        #region 状态属性
        
        /// <summary>
        /// 当前连接的设备类型
        /// </summary>
        DeviceType CurrentConnectedDevice { get; }
        
        /// <summary>
        /// VDC-32 是否已连接
        /// </summary>
        bool IsVdc32Connected { get; }
        
        /// <summary>
        /// 负载设备是否已连接
        /// </summary>
        bool IsLoadDeviceConnected { get; }
        
        /// <summary>
        /// 是否有任何设备连接
        /// </summary>
        bool HasActiveConnection { get; }
        
        #endregion

        #region 连接操作
        
        /// <summary>
        /// 连接 VDC-32 设备 (串口)
        /// </summary>
        /// <param name="port">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="slaveId">从机地址</param>
        /// <returns>是否成功</returns>
        Task<bool> ConnectVdc32SerialAsync(string port, int baudRate, byte slaveId);
        
        /// <summary>
        /// 连接 VDC-32 设备 (TCP)
        /// </summary>
        /// <param name="ip">IP 地址</param>
        /// <param name="port">端口</param>
        /// <param name="slaveId">从机地址</param>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <returns>是否成功</returns>
        Task<bool> ConnectVdc32TcpAsync(string ip, int port, byte slaveId, int timeout = 5000);
        
        /// <summary>
        /// 连接负载设备 (仅串口)
        /// </summary>
        /// <param name="port">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <returns>是否成功</returns>
        Task<bool> ConnectLoadDeviceAsync(string port, int baudRate);
        
        /// <summary>
        /// 断开当前设备
        /// </summary>
        Task DisconnectCurrentAsync();
        
        /// <summary>
        /// 断开指定类型的设备
        /// </summary>
        /// <param name="deviceType">设备类型</param>
        Task DisconnectAsync(DeviceType deviceType);
        
        /// <summary>
        /// 断开所有设备
        /// </summary>
        Task DisconnectAllAsync();
        
        #endregion

        #region 事件
        
        /// <summary>
        /// 连接状态变更事件
        /// </summary>
        event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        
        /// <summary>
        /// 连接前将要断开其他设备事件
        /// </summary>
        event EventHandler<DeviceType> BeforeDisconnectOther;
        
        /// <summary>
        /// 连接失败事件
        /// </summary>
        event EventHandler<Exception> ConnectionFailed;
        
        #endregion
    }
}
