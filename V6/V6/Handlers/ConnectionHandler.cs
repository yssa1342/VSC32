using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Handlers
{
    /// <summary>
    /// 连接处理器
    /// 职责：处理设备连接/断开的业务逻辑
    /// </summary>
    public class ConnectionHandler
    {
        #region 常量定义

        private const int CONNECTION_TIMEOUT_MS = 5000;
        private const int DISCONNECT_DELAY_MS = 100;

        #endregion

        #region 私有字段

        private readonly IDeviceConnectionCoordinator _connectionCoordinator;
        private readonly IPollingCoordinator _pollingCoordinator;
        private readonly IUIStateCoordinator _uiStateCoordinator;
        private readonly Action<string, bool?> _logAction;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建连接处理器
        /// </summary>
        public ConnectionHandler(
            IDeviceConnectionCoordinator connectionCoordinator,
            IPollingCoordinator pollingCoordinator,
            IUIStateCoordinator uiStateCoordinator,
            Action<string, bool?> logAction)
        {
            _connectionCoordinator = connectionCoordinator ?? throw new ArgumentNullException(nameof(connectionCoordinator));
            _pollingCoordinator = pollingCoordinator ?? throw new ArgumentNullException(nameof(pollingCoordinator));
            _uiStateCoordinator = uiStateCoordinator ?? throw new ArgumentNullException(nameof(uiStateCoordinator));
            _logAction = logAction ?? ((msg, success) => { });
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 处理 VDC-32 串口连接请求
        /// </summary>
        public async Task<ConnectionResult> HandleVdc32SerialConnectAsync(
            string port,
            int baudRate,
            byte slaveId)
        {
            if (string.IsNullOrWhiteSpace(port))
            {
                return ConnectionResult.Fail("请选择串口");
            }

            _logAction($"正在连接 VDC-32 (串口: {port}, 波特率: {baudRate})...", null);
            _uiStateCoordinator.ShowLoading("正在连接...");

            try
            {
                // 如果正在轮询，先停止
                if (_pollingCoordinator.IsPolling)
                {
                    await _pollingCoordinator.StopAsync(waitForComplete: true);
                }

                bool success = await _connectionCoordinator.ConnectVdc32SerialAsync(port, baudRate, slaveId);

                if (success)
                {
                    _logAction($"VDC-32 连接成功 ({port})", true);
                    _uiStateCoordinator.UpdateConnectionState(DeviceType.VDC32, true);
                    return ConnectionResult.Ok("连接成功");
                }
                else
                {
                    _logAction($"VDC-32 连接失败", false);
                    return ConnectionResult.Fail("连接失败，请检查设备和串口设置");
                }
            }
            catch (Exception ex)
            {
                _logAction($"连接异常: {ex.Message}", false);
                return ConnectionResult.Fail($"连接异常: {ex.Message}");
            }
            finally
            {
                _uiStateCoordinator.HideLoading();
            }
        }

        /// <summary>
        /// 处理 VDC-32 TCP 连接请求
        /// </summary>
        public async Task<ConnectionResult> HandleVdc32TcpConnectAsync(
            string ip,
            int port,
            byte slaveId)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return ConnectionResult.Fail("请输入 IP 地址");
            }

            if (!IsValidIpAddress(ip))
            {
                return ConnectionResult.Fail("IP 地址格式无效");
            }

            _logAction($"正在连接 VDC-32 (TCP: {ip}:{port})...", null);
            _uiStateCoordinator.ShowLoading("正在连接...");

            try
            {
                if (_pollingCoordinator.IsPolling)
                {
                    await _pollingCoordinator.StopAsync(waitForComplete: true);
                }

                bool success = await _connectionCoordinator.ConnectVdc32TcpAsync(
                    ip, port, slaveId, CONNECTION_TIMEOUT_MS);

                if (success)
                {
                    _logAction($"VDC-32 连接成功 ({ip}:{port})", true);
                    _uiStateCoordinator.UpdateConnectionState(DeviceType.VDC32, true);
                    return ConnectionResult.Ok("连接成功");
                }
                else
                {
                    _logAction($"VDC-32 TCP 连接失败", false);
                    return ConnectionResult.Fail("连接失败，请检查网络设置");
                }
            }
            catch (Exception ex)
            {
                _logAction($"TCP 连接异常: {ex.Message}", false);
                return ConnectionResult.Fail($"连接异常: {ex.Message}");
            }
            finally
            {
                _uiStateCoordinator.HideLoading();
            }
        }

        /// <summary>
        /// 处理负载设备连接请求
        /// </summary>
        public async Task<ConnectionResult> HandleLoadDeviceConnectAsync(
            string port,
            int baudRate)
        {
            if (string.IsNullOrWhiteSpace(port))
            {
                return ConnectionResult.Fail("请选择串口");
            }

            _logAction($"正在连接 GJDD-750 (串口: {port})...", null);
            _uiStateCoordinator.ShowLoading("正在连接...");

            try
            {
                if (_pollingCoordinator.IsPolling)
                {
                    await _pollingCoordinator.StopAsync(waitForComplete: true);
                }

                bool success = await _connectionCoordinator.ConnectLoadDeviceAsync(port, baudRate);

                if (success)
                {
                    _logAction($"GJDD-750 连接成功 ({port})", true);
                    _uiStateCoordinator.UpdateConnectionState(DeviceType.LoadDevice, true);
                    return ConnectionResult.Ok("连接成功");
                }
                else
                {
                    _logAction($"GJDD-750 连接失败", false);
                    return ConnectionResult.Fail("连接失败，请检查设备");
                }
            }
            catch (Exception ex)
            {
                _logAction($"连接异常: {ex.Message}", false);
                return ConnectionResult.Fail($"连接异常: {ex.Message}");
            }
            finally
            {
                _uiStateCoordinator.HideLoading();
            }
        }

        /// <summary>
        /// 处理断开连接请求
        /// </summary>
        public async Task<ConnectionResult> HandleDisconnectAsync()
        {
            var currentDevice = _connectionCoordinator.CurrentConnectedDevice;

            if (currentDevice == DeviceType.None)
            {
                return ConnectionResult.Ok("无设备连接");
            }

            _logAction($"正在断开 {GetDeviceName(currentDevice)}...", null);

            try
            {
                // 先停止轮询
                if (_pollingCoordinator.IsPolling)
                {
                    await _pollingCoordinator.StopAsync(waitForComplete: true);
                }

                await Task.Delay(DISCONNECT_DELAY_MS);

                await _connectionCoordinator.DisconnectCurrentAsync();

                _uiStateCoordinator.UpdateConnectionState(currentDevice, false);
                _uiStateCoordinator.ResetAllStatusIndicators();
                _uiStateCoordinator.ClearDeviceInfo();

                _logAction($"{GetDeviceName(currentDevice)} 已断开", true);
                return ConnectionResult.Ok("已断开连接");
            }
            catch (Exception ex)
            {
                _logAction($"断开连接异常: {ex.Message}", false);
                return ConnectionResult.Fail($"断开异常: {ex.Message}");
            }
        }

        #endregion

        #region 私有方法

        private bool IsValidIpAddress(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return false;

            string[] parts = ip.Split('.');
            if (parts.Length != 4)
                return false;

            foreach (string part in parts)
            {
                if (!byte.TryParse(part, out _))
                    return false;
            }

            return true;
        }

        private string GetDeviceName(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.VDC32:
                    return "VDC-32";
                case DeviceType.LoadDevice:
                    return "GJDD-750";
                default:
                    return "设备";
            }
        }

        #endregion
    }

    /// <summary>
    /// 连接结果
    /// </summary>
    public class ConnectionResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        private ConnectionResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static ConnectionResult Ok(string message) => new ConnectionResult(true, message);
        public static ConnectionResult Fail(string message) => new ConnectionResult(false, message);
    }
}
