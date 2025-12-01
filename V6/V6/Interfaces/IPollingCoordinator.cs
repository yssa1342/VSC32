using System;
using System.Threading.Tasks;

namespace GJVdc32Tool.Interfaces
{
    /// <summary>
    /// 轮询状态枚举
    /// </summary>
    public enum PollingState
    {
        /// <summary>
        /// 已停止
        /// </summary>
        Stopped,
        
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        
        /// <summary>
        /// 已暂停
        /// </summary>
        Paused,
        
        /// <summary>
        /// 正在停止
        /// </summary>
        Stopping
    }

    /// <summary>
    /// 轮询数据事件参数
    /// </summary>
    public class PollingDataEventArgs : EventArgs
    {
        /// <summary>
        /// 轮询周期序号
        /// </summary>
        public long CycleNumber { get; set; }
        
        /// <summary>
        /// 是否有有效数据
        /// </summary>
        public bool HasValidData { get; set; }
        
        /// <summary>
        /// 轮询耗时(毫秒)
        /// </summary>
        public int ElapsedMs { get; set; }
    }

    /// <summary>
    /// 轮询协调器接口
    /// 负责管理设备数据轮询的生命周期
    /// </summary>
    public interface IPollingCoordinator
    {
        #region 状态属性
        
        /// <summary>
        /// 当前轮询状态
        /// </summary>
        PollingState State { get; }
        
        /// <summary>
        /// 是否正在轮询
        /// </summary>
        bool IsPolling { get; }
        
        /// <summary>
        /// 轮询间隔(毫秒)
        /// </summary>
        int PollingInterval { get; set; }
        
        /// <summary>
        /// 已完成的轮询周期数
        /// </summary>
        long CompletedCycles { get; }
        
        /// <summary>
        /// 连续错误次数
        /// </summary>
        int ConsecutiveErrors { get; }
        
        #endregion

        #region 轮询控制
        
        /// <summary>
        /// 启动 VDC-32 数据轮询
        /// </summary>
        Task StartVdc32PollingAsync();
        
        /// <summary>
        /// 启动负载设备数据轮询
        /// </summary>
        Task StartLoadDevicePollingAsync();
        
        /// <summary>
        /// 停止轮询
        /// </summary>
        /// <param name="waitForComplete">是否等待当前周期完成</param>
        Task StopAsync(bool waitForComplete = true);
        
        /// <summary>
        /// 暂停轮询
        /// </summary>
        void Pause();
        
        /// <summary>
        /// 恢复轮询
        /// </summary>
        void Resume();
        
        /// <summary>
        /// 暂停轮询并执行操作，完成后自动恢复
        /// </summary>
        /// <param name="action">要执行的操作</param>
        Task PauseAndExecuteAsync(Func<Task> action);
        
        #endregion

        #region 事件
        
        /// <summary>
        /// 轮询周期完成事件
        /// </summary>
        event EventHandler<PollingDataEventArgs> PollingCycleCompleted;
        
        /// <summary>
        /// 轮询错误事件
        /// </summary>
        event EventHandler<Exception> PollingError;
        
        /// <summary>
        /// 轮询状态变更事件
        /// </summary>
        event EventHandler<PollingState> StateChanged;
        
        /// <summary>
        /// 连续错误达到阈值事件
        /// </summary>
        event EventHandler<int> ErrorThresholdReached;
        
        #endregion
    }
}
