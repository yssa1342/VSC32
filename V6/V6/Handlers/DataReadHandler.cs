using System;
using System.Threading;
using System.Threading.Tasks;

namespace GJVdc32Tool.Handlers
{
    /// <summary>
    /// 数据读取处理器
    /// 职责：处理设备数据读取逻辑，转换原始数据为业务模型
    /// </summary>
    public class DataReadHandler
    {
        #region 常量定义

        /// <summary>
        /// VDC-32 通道数
        /// </summary>
        public const int VDC32_CHANNEL_COUNT = 32;

        /// <summary>
        /// 负载设备通道数
        /// </summary>
        public const int LOAD_CHANNEL_COUNT = 8;

        /// <summary>
        /// 电压转换系数 (mV -> V)
        /// </summary>
        private const double MV_TO_V = 0.001;

        /// <summary>
        /// 电流转换系数
        /// </summary>
        private const double CURRENT_SCALE = 0.01;

        #endregion

        #region 私有字段

        private readonly Func<CancellationToken, Task<ushort[]>> _readVdc32Registers;
        private readonly Func<CancellationToken, Task<LoadChannelData[]>> _readLoadChannels;
        private readonly Action<string, bool?> _logAction;

        #endregion

        #region 构造函数

        /// <summary>
        /// 创建数据读取处理器
        /// </summary>
        /// <param name="readVdc32Registers">VDC-32 寄存器读取委托</param>
        /// <param name="readLoadChannels">负载设备通道读取委托</param>
        /// <param name="logAction">日志记录委托</param>
        public DataReadHandler(
            Func<CancellationToken, Task<ushort[]>> readVdc32Registers,
            Func<CancellationToken, Task<LoadChannelData[]>> readLoadChannels,
            Action<string, bool?> logAction = null)
        {
            _readVdc32Registers = readVdc32Registers ?? throw new ArgumentNullException(nameof(readVdc32Registers));
            _readLoadChannels = readLoadChannels ?? throw new ArgumentNullException(nameof(readLoadChannels));
            _logAction = logAction ?? ((msg, success) => { });
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 读取 VDC-32 所有通道电压
        /// </summary>
        /// <returns>通道电压数据</returns>
        public async Task<Vdc32ReadResult> ReadVdc32ChannelsAsync(CancellationToken token)
        {
            try
            {
                var registers = await _readVdc32Registers(token);

                if (registers == null || registers.Length < VDC32_CHANNEL_COUNT)
                {
                    return Vdc32ReadResult.Fail("寄存器数据不完整");
                }

                var voltages = new double[VDC32_CHANNEL_COUNT];
                var alarms = new bool[VDC32_CHANNEL_COUNT];

                for (int i = 0; i < VDC32_CHANNEL_COUNT; i++)
                {
                    voltages[i] = ConvertToVoltage(registers[i]);
                    alarms[i] = CheckVoltageAlarm(voltages[i]);
                }

                return Vdc32ReadResult.Ok(voltages, alarms);
            }
            catch (OperationCanceledException)
            {
                return Vdc32ReadResult.Cancelled();
            }
            catch (Exception ex)
            {
                _logAction($"读取 VDC-32 数据异常: {ex.Message}", false);
                return Vdc32ReadResult.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 读取负载设备所有通道数据
        /// </summary>
        public async Task<LoadDeviceReadResult> ReadLoadChannelsAsync(CancellationToken token)
        {
            try
            {
                var channels = await _readLoadChannels(token);

                if (channels == null || channels.Length < LOAD_CHANNEL_COUNT)
                {
                    return LoadDeviceReadResult.Fail("通道数据不完整");
                }

                return LoadDeviceReadResult.Ok(channels);
            }
            catch (OperationCanceledException)
            {
                return LoadDeviceReadResult.Cancelled();
            }
            catch (Exception ex)
            {
                _logAction($"读取负载设备数据异常: {ex.Message}", false);
                return LoadDeviceReadResult.Fail(ex.Message);
            }
        }

        /// <summary>
        /// 解析 IO 状态位
        /// </summary>
        /// <param name="ioRegister">IO 状态寄存器值</param>
        /// <returns>各 IO 点状态数组</returns>
        public bool[] ParseIoStatus(ushort ioRegister)
        {
            var status = new bool[16];
            for (int i = 0; i < 16; i++)
            {
                status[i] = (ioRegister & (1 << i)) != 0;
            }
            return status;
        }

        /// <summary>
        /// 解析环境数据（温度、湿度）
        /// </summary>
        public EnvironmentData ParseEnvironmentData(ushort tempRegister, ushort humidRegister)
        {
            return new EnvironmentData
            {
                Temperature = ConvertToTemperature(tempRegister),
                Humidity = ConvertToHumidity(humidRegister)
            };
        }

        #endregion

        #region 私有方法

        private double ConvertToVoltage(ushort rawValue)
        {
            // 有符号处理
            short signedValue = (short)rawValue;
            return signedValue * MV_TO_V;
        }

        private bool CheckVoltageAlarm(double voltage)
        {
            // 电压超出正常范围视为报警
            return voltage < -50.0 || voltage > 50.0;
        }

        private double ConvertToTemperature(ushort rawValue)
        {
            short signedValue = (short)rawValue;
            return signedValue * 0.1;
        }

        private double ConvertToHumidity(ushort rawValue)
        {
            return rawValue * 0.1;
        }

        #endregion
    }

    #region 结果类

    /// <summary>
    /// VDC-32 读取结果
    /// </summary>
    public class Vdc32ReadResult
    {
        public bool Success { get; private set; }
        public bool IsCancelled { get; private set; }
        public string ErrorMessage { get; private set; }
        public double[] Voltages { get; private set; }
        public bool[] Alarms { get; private set; }

        private Vdc32ReadResult() { }

        public static Vdc32ReadResult Ok(double[] voltages, bool[] alarms)
        {
            return new Vdc32ReadResult
            {
                Success = true,
                Voltages = voltages,
                Alarms = alarms
            };
        }

        public static Vdc32ReadResult Fail(string message)
        {
            return new Vdc32ReadResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        public static Vdc32ReadResult Cancelled()
        {
            return new Vdc32ReadResult
            {
                Success = false,
                IsCancelled = true
            };
        }
    }

    /// <summary>
    /// 负载设备读取结果
    /// </summary>
    public class LoadDeviceReadResult
    {
        public bool Success { get; private set; }
        public bool IsCancelled { get; private set; }
        public string ErrorMessage { get; private set; }
        public LoadChannelData[] Channels { get; private set; }

        private LoadDeviceReadResult() { }

        public static LoadDeviceReadResult Ok(LoadChannelData[] channels)
        {
            return new LoadDeviceReadResult
            {
                Success = true,
                Channels = channels
            };
        }

        public static LoadDeviceReadResult Fail(string message)
        {
            return new LoadDeviceReadResult
            {
                Success = false,
                ErrorMessage = message
            };
        }

        public static LoadDeviceReadResult Cancelled()
        {
            return new LoadDeviceReadResult
            {
                Success = false,
                IsCancelled = true
            };
        }
    }

    /// <summary>
    /// 负载通道数据
    /// </summary>
    public class LoadChannelData
    {
        public int ChannelIndex { get; set; }
        public double Current { get; set; }
        public double Voltage { get; set; }
        public double Power { get; set; }
        public bool IsOn { get; set; }
        public bool HasFault { get; set; }
        public string FaultMessage { get; set; }
    }

    /// <summary>
    /// 环境数据
    /// </summary>
    public class EnvironmentData
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }

    #endregion
}
