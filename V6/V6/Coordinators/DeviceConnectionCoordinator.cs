using System;
using System.Threading.Tasks;
using GJVdc32Tool.Interfaces;

namespace GJVdc32Tool.Coordinators
{
    /// <summary>
    /// 设备连接协调器
    /// 职责：管理 VDC-32 和 GJDD-750 的连接互斥，确保同一时间只有一个设备连接
    /// </summary>
    /// <remarks>
    /// 架构规则：
    /// - 不直接操作 UI 控件
    /// - 通过事件通知状态变更
    /// - 依赖注入设备服务
    /// </remarks>
    public class DeviceConnectionCoordinator : IDeviceConnectionCoordinator
    {
        #region 私有字段

        private readonly object _lockObject = new object();
        private DeviceType _currentDevice = DeviceType.None;
        private bool _vdc32Connected = false;
        private bool _loadDeviceConnected = false;

        // 设备服务委托（避免直接依赖具体实现）
        private readonly Func<string, int, byte, bool> _connectVdc32Serial;
        private readonly Func<string, int, byte, int, Task<bool>> _connectVdc32Tcp;
        private readonly Func<string, int, bool> _connectLoadDevice;
        private readonly Func<Task> _disconnectVdc32;
        private readonly Func<Task> _disconnectLoadDevice;
        private readonly Func<bool> _isVdc32Connected;
        private readonly Func<bool> _isLoadDeviceConnected;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建设备连接协调器
        /// </summary>
        /// <param name="connectVdc32Serial">VDC-32 串口连接委托</param>
        /// <param name="connectVdc32Tcp">VDC-32 TCP连接委托</param>
        /// <param name="connectLoadDevice">负载设备连接委托</param>
        /// <param name="disconnectVdc32">VDC-32 断开委托</param>
        /// <param name="disconnectLoadDevice">负载设备断开委托</param>
        /// <param name="isVdc32Connected">VDC-32 连接状态查询</param>
        /// <param name="isLoadDeviceConnected">负载设备连接状态查询</param>
        public DeviceConnectionCoordinator(
            Func<string, int, byte, bool> connectVdc32Serial,
            Func<string, int, byte, int, Task<bool>> connectVdc32Tcp,
            Func<string, int, bool> connectLoadDevice,
            Func<Task> disconnectVdc32,
            Func<Task> disconnectLoadDevice,
            Func<bool> isVdc32Connected,
            Func<bool> isLoadDeviceConnected)
        {
            _connectVdc32Serial = connectVdc32Serial ?? throw new ArgumentNullException(nameof(connectVdc32Serial));
            _connectVdc32Tcp = connectVdc32Tcp ?? throw new ArgumentNullException(nameof(connectVdc32Tcp));
            _connectLoadDevice = connectLoadDevice ?? throw new ArgumentNullException(nameof(connectLoadDevice));
            _disconnectVdc32 = disconnectVdc32 ?? throw new ArgumentNullException(nameof(disconnectVdc32));
            _disconnectLoadDevice = disconnectLoadDevice ?? throw new ArgumentNullException(nameof(disconnectLoadDevice));
            _isVdc32Connected = isVdc32Connected ?? throw new ArgumentNullException(nameof(isVdc32Connected));
            _isLoadDeviceConnected = isLoadDeviceConnected ?? throw new ArgumentNullException(nameof(isLoadDeviceConnected));
        }

        #endregion

        #region IDeviceConnectionCoordinator 实现

        /// <inheritdoc/>
        public DeviceType CurrentConnectedDevice
        {
            get
            {
                lock (_lockObject)
                {
                    return _currentDevice;
                }
            }
        }

        /// <inheritdoc/>
        public bool IsVdc32Connected
        {
            get
            {
                lock (_lockObject)
                {
                    return _vdc32Connected && _isVdc32Connected();
                }
            }
        }

        /// <inheritdoc/>
        public bool IsLoadDeviceConnected
        {
            get
            {
                lock (_lockObject)
                {
                    return _loadDeviceConnected && _isLoadDeviceConnected();
                }
            }
        }

        /// <inheritdoc/>
        public bool HasActiveConnection => IsVdc32Connected || IsLoadDeviceConnected;

        /// <inheritdoc/>
        public async Task<bool> ConnectVdc32SerialAsync(string port, int baudRate, byte slaveId)
        {
            ValidateSerialParams(port, baudRate);

            // 互斥：先断开负载设备
            if (IsLoadDeviceConnected)
            {
                OnBeforeDisconnectOther(DeviceType.LoadDevice);
                await DisconnectLoadDeviceInternalAsync();
            }

            try
            {
                bool success = _connectVdc32Serial(port, baudRate, slaveId);
                
                if (success)
                {
                    lock (_lockObject)
                    {
                        _vdc32Connected = true;
                        _currentDevice = DeviceType.VDC32;
                    }

                    OnConnectionStateChanged(new ConnectionStateChangedEventArgs
                    {
                        DeviceType = DeviceType.VDC32,
                        IsConnected = true,
                        ConnectionInfo = $"串口: {port} @ {baudRate}bps, Slave ID: {slaveId}"
                    });
                }

                return success;
            }
            catch (Exception ex)
            {
                OnConnectionFailed(ex);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ConnectVdc32TcpAsync(string ip, int port, byte slaveId, int timeout = 5000)
        {
            ValidateTcpParams(ip, port);

            // 互斥：先断开负载设备
            if (IsLoadDeviceConnected)
            {
                OnBeforeDisconnectOther(DeviceType.LoadDevice);
                await DisconnectLoadDeviceInternalAsync();
            }

            try
            {
                bool success = await _connectVdc32Tcp(ip, port, slaveId, timeout);
                
                if (success)
                {
                    lock (_lockObject)
                    {
                        _vdc32Connected = true;
                        _currentDevice = DeviceType.VDC32;
                    }

                    OnConnectionStateChanged(new ConnectionStateChangedEventArgs
                    {
                        DeviceType = DeviceType.VDC32,
                        IsConnected = true,
                        ConnectionInfo = $"TCP: {ip}:{port}, Slave ID: {slaveId}"
                    });
                }

                return success;
            }
            catch (Exception ex)
            {
                OnConnectionFailed(ex);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ConnectLoadDeviceAsync(string port, int baudRate)
        {
            ValidateSerialParams(port, baudRate);

            // 互斥：先断开 VDC-32
            if (IsVdc32Connected)
            {
                OnBeforeDisconnectOther(DeviceType.VDC32);
                await DisconnectVdc32InternalAsync();
            }

            try
            {
                bool success = _connectLoadDevice(port, baudRate);
                
                if (success)
                {
                    lock (_lockObject)
                    {
                        _loadDeviceConnected = true;
                        _currentDevice = DeviceType.LoadDevice;
                    }

                    OnConnectionStateChanged(new ConnectionStateChangedEventArgs
                    {
                        DeviceType = DeviceType.LoadDevice,
                        IsConnected = true,
                        ConnectionInfo = $"串口: {port} @ {baudRate}bps (EE协议)"
                    });
                }

                return success;
            }
            catch (Exception ex)
            {
                OnConnectionFailed(ex);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task DisconnectCurrentAsync()
        {
            await DisconnectAsync(CurrentConnectedDevice);
        }

        /// <inheritdoc/>
        public async Task DisconnectAsync(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.VDC32:
                    await DisconnectVdc32InternalAsync();
                    break;
                case DeviceType.LoadDevice:
                    await DisconnectLoadDeviceInternalAsync();
                    break;
                case DeviceType.None:
                    // 无需操作
                    break;
            }
        }

        /// <inheritdoc/>
        public async Task DisconnectAllAsync()
        {
            await DisconnectVdc32InternalAsync();
            await DisconnectLoadDeviceInternalAsync();
        }

        #endregion

        #region 事件

        /// <inheritdoc/>
        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        /// <inheritdoc/>
        public event EventHandler<DeviceType> BeforeDisconnectOther;

        /// <inheritdoc/>
        public event EventHandler<Exception> ConnectionFailed;

        #endregion

        #region 私有方法

        private async Task DisconnectVdc32InternalAsync()
        {
            if (!_vdc32Connected) return;

            try
            {
                await _disconnectVdc32();
            }
            finally
            {
                lock (_lockObject)
                {
                    _vdc32Connected = false;
                    if (_currentDevice == DeviceType.VDC32)
                    {
                        _currentDevice = DeviceType.None;
                    }
                }

                OnConnectionStateChanged(new ConnectionStateChangedEventArgs
                {
                    DeviceType = DeviceType.VDC32,
                    IsConnected = false,
                    ConnectionInfo = "已断开"
                });
            }
        }

        private async Task DisconnectLoadDeviceInternalAsync()
        {
            if (!_loadDeviceConnected) return;

            try
            {
                await _disconnectLoadDevice();
            }
            finally
            {
                lock (_lockObject)
                {
                    _loadDeviceConnected = false;
                    if (_currentDevice == DeviceType.LoadDevice)
                    {
                        _currentDevice = DeviceType.None;
                    }
                }

                OnConnectionStateChanged(new ConnectionStateChangedEventArgs
                {
                    DeviceType = DeviceType.LoadDevice,
                    IsConnected = false,
                    ConnectionInfo = "已断开"
                });
            }
        }

        private void ValidateSerialParams(string port, int baudRate)
        {
            if (string.IsNullOrWhiteSpace(port))
            {
                throw new ArgumentException("串口名称不能为空", nameof(port));
            }

            int[] validBaudRates = { 9600, 19200, 38400, 57600, 115200 };
            if (Array.IndexOf(validBaudRates, baudRate) < 0)
            {
                throw new ArgumentException($"无效的波特率: {baudRate}", nameof(baudRate));
            }
        }

        private void ValidateTcpParams(string ip, int port)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new ArgumentException("IP 地址不能为空", nameof(ip));
            }

            if (port < 1 || port > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(port), "端口范围: 1-65535");
            }
        }

        private void OnConnectionStateChanged(ConnectionStateChangedEventArgs e)
        {
            ConnectionStateChanged?.Invoke(this, e);
        }

        private void OnBeforeDisconnectOther(DeviceType deviceType)
        {
            BeforeDisconnectOther?.Invoke(this, deviceType);
        }

        private void OnConnectionFailed(Exception ex)
        {
            ConnectionFailed?.Invoke(this, ex);
        }

        #endregion
    }
}
