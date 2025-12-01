using System;
using System.Drawing;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// UI 状态协调器接口
    /// 负责统一管理界面状态的更新
    /// </summary>
    public interface IUIStateCoordinator
    {
        #region 连接状态 UI
        
        /// <summary>
        /// 更新连接状态 UI
        /// </summary>
        /// <param name="deviceType">设备类型</param>
        /// <param name="isConnected">是否已连接</param>
        void UpdateConnectionState(DeviceType deviceType, bool isConnected);
        
        /// <summary>
        /// 更新连接按钮状态
        /// </summary>
        /// <param name="text">按钮文本</param>
        /// <param name="color">按钮颜色</param>
        /// <param name="enabled">是否启用</param>
        void UpdateConnectButton(string text, Color color, bool enabled = true);
        
        #endregion

        #region 控件启用状态
        
        /// <summary>
        /// 启用/禁用 VDC-32 相关控件
        /// </summary>
        void EnableVdc32Controls(bool enabled);
        
        /// <summary>
        /// 启用/禁用负载设备相关控件
        /// </summary>
        void EnableLoadDeviceControls(bool enabled);
        
        /// <summary>
        /// 启用/禁用连接配置控件
        /// </summary>
        void EnableConnectionControls(bool enabled);
        
        /// <summary>
        /// 启用/禁用所有操作控件
        /// </summary>
        void EnableAllOperationControls(bool enabled);
        
        #endregion

        #region 状态指示器
        
        /// <summary>
        /// 重置所有状态指示器
        /// </summary>
        void ResetAllStatusIndicators();
        
        /// <summary>
        /// 更新设备信息显示
        /// </summary>
        /// <param name="version">固件版本</param>
        /// <param name="name">设备名称</param>
        /// <param name="address">从机地址</param>
        void UpdateDeviceInfo(string version, string name, string address);
        
        /// <summary>
        /// 清除设备信息显示
        /// </summary>
        void ClearDeviceInfo();
        
        #endregion

        #region 视图切换
        
        /// <summary>
        /// 准备切换视图
        /// </summary>
        /// <param name="targetView">目标视图</param>
        void PrepareViewSwitch(string targetView);
        
        /// <summary>
        /// 完成视图切换
        /// </summary>
        /// <param name="currentView">当前视图</param>
        void CompleteViewSwitch(string currentView);
        
        #endregion

        #region 加载状态
        
        /// <summary>
        /// 显示加载中状态
        /// </summary>
        /// <param name="message">加载消息</param>
        void ShowLoading(string message);
        
        /// <summary>
        /// 隐藏加载状态
        /// </summary>
        void HideLoading();
        
        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="progress">进度值 (0-100)</param>
        /// <param name="message">进度消息</param>
        void UpdateProgress(int progress, string message);
        
        #endregion

        #region 事件
        
        /// <summary>
        /// UI 状态变更事件
        /// </summary>
        event EventHandler<string> UIStateChanged;
        
        #endregion
    }
}
