using System;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// 连接配置面板接口
    /// 定义连接配置视图需要实现的功能
    /// </summary>
    public interface IConnectionPanel
    {
        #region 连接方式
        
        /// <summary>
        /// 是否使用串口连接
        /// </summary>
        bool UseSerialConnection { get; set; }
        
        /// <summary>
        /// 是否使用 TCP 连接
        /// </summary>
        bool UseTcpConnection { get; set; }
        
        /// <summary>
        /// 串口选项是否可用
        /// </summary>
        bool SerialOptionEnabled { get; set; }
        
        /// <summary>
        /// TCP 选项是否可用
        /// </summary>
        bool TcpOptionEnabled { get; set; }
        
        #endregion

        #region 串口配置
        
        /// <summary>
        /// 选中的串口名称
        /// </summary>
        string SelectedPort { get; set; }
        
        /// <summary>
        /// 可用串口列表
        /// </summary>
        string[] AvailablePorts { set; }
        
        /// <summary>
        /// 选中的波特率
        /// </summary>
        int SelectedBaudRate { get; set; }
        
        /// <summary>
        /// 串口控件是否启用
        /// </summary>
        bool SerialControlsEnabled { set; }
        
        #endregion

        #region TCP 配置
        
        /// <summary>
        /// TCP IP 地址
        /// </summary>
        string TcpIpAddress { get; set; }
        
        /// <summary>
        /// TCP 端口
        /// </summary>
        int TcpPort { get; set; }
        
        /// <summary>
        /// TCP 控件是否启用
        /// </summary>
        bool TcpControlsEnabled { set; }
        
        #endregion

        #region 从机地址
        
        /// <summary>
        /// 从机地址
        /// </summary>
        byte SlaveId { get; set; }
        
        /// <summary>
        /// 从机地址控件是否启用
        /// </summary>
        bool SlaveIdEnabled { set; }
        
        /// <summary>
        /// 应用地址按钮是否可见
        /// </summary>
        bool ApplySlaveIdVisible { set; }
        
        #endregion

        #region 连接状态
        
        /// <summary>
        /// 连接按钮文本
        /// </summary>
        string ConnectButtonText { set; }
        
        /// <summary>
        /// 连接按钮背景色
        /// </summary>
        System.Drawing.Color ConnectButtonColor { set; }
        
        /// <summary>
        /// 连接状态文本
        /// </summary>
        string ConnectionStatusText { set; }
        
        /// <summary>
        /// 连接状态颜色
        /// </summary>
        System.Drawing.Color ConnectionStatusColor { set; }
        
        /// <summary>
        /// 面板标题
        /// </summary>
        string PanelTitle { set; }
        
        #endregion

        #region 事件
        
        /// <summary>
        /// 连接/断开按钮点击事件
        /// </summary>
        event EventHandler ConnectClicked;
        
        /// <summary>
        /// 刷新串口列表事件
        /// </summary>
        event EventHandler RefreshPortsClicked;
        
        /// <summary>
        /// 连接方式改变事件
        /// </summary>
        event EventHandler<bool> ConnectionModeChanged;
        
        /// <summary>
        /// 应用从机地址事件
        /// </summary>
        event EventHandler ApplySlaveIdClicked;
        
        #endregion

        #region 方法
        
        /// <summary>
        /// 刷新可用串口列表
        /// </summary>
        void RefreshPorts();
        
        /// <summary>
        /// 显示串口相关控件
        /// </summary>
        void ShowSerialControls();
        
        /// <summary>
        /// 显示 TCP 相关控件
        /// </summary>
        void ShowTcpControls();
        
        #endregion
    }
}
