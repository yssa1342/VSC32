using System;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// 主视图接口
    /// 定义主窗体需要实现的功能
    /// </summary>
    public interface IMainView
    {
        #region 视图切换
        
        /// <summary>
        /// 当前活动视图名称
        /// </summary>
        string CurrentView { get; }
        
        /// <summary>
        /// 切换到指定视图
        /// </summary>
        /// <param name="viewName">视图名称: "VDC32", "LOAD", "LOG"</param>
        void SwitchView(string viewName);
        
        #endregion

        #region 状态栏
        
        /// <summary>
        /// 显示状态消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="success">是否成功 (null=信息, true=成功, false=失败)</param>
        void ShowStatus(string message, bool? success = null);
        
        /// <summary>
        /// 显示底部实时日志
        /// </summary>
        void ShowRealTimeLog(string message, bool? success = null);
        
        #endregion

        #region 菜单状态
        
        /// <summary>
        /// 更新菜单按钮激活状态
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="isActive">是否激活</param>
        void UpdateMenuButtonState(string viewName, bool isActive);
        
        #endregion

        #region 日志
        
        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="message">日志消息</param>
        void AppendLog(string message);
        
        /// <summary>
        /// 清空日志
        /// </summary>
        void ClearLog();
        
        #endregion

        #region 子视图访问
        
        /// <summary>
        /// VDC-32 视图
        /// </summary>
        IVdc32View Vdc32View { get; }
        
        /// <summary>
        /// 负载设备视图
        /// </summary>
        ILoadDeviceView LoadDeviceView { get; }
        
        /// <summary>
        /// 连接配置面板
        /// </summary>
        IConnectionPanel ConnectionPanel { get; }
        
        #endregion

        #region 事件
        
        /// <summary>
        /// 视图切换事件
        /// </summary>
        event EventHandler<string> ViewSwitched;
        
        /// <summary>
        /// 窗体加载完成事件
        /// </summary>
        event EventHandler ViewLoaded;
        
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        event EventHandler ViewClosing;
        
        #endregion
    }
}
