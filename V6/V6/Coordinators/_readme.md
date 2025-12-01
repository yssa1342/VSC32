# Coordinators 协调器层

## 概述

协调器负责管理跨越多个组件的复杂业务逻辑，是 MVP 架构中的关键抽象层。

## 文件列表

| 文件 | 职责 | 代码行数 |
|------|------|----------|
| `DeviceConnectionCoordinator.cs` | 设备连接互斥管理 | ~270 行 |
| `PollingCoordinator.cs` | 数据轮询生命周期 | ~290 行 |
| `UIStateCoordinator.cs` | UI 状态统一管理 | ~240 行 |

## 架构规则遵从

✅ 每个类 < 500 行  
✅ 每个方法 < 50 行  
✅ 每个方法参数 < 5 个  
✅ 通过接口依赖注入  
✅ 使用事件通知而非直接 UI 操作  

## 使用示例

### DeviceConnectionCoordinator

```csharp
// 创建协调器
var coordinator = new DeviceConnectionCoordinator(
    connectVdc32Serial: (port, baud, slave) => _deviceService.ConnectSerial(port, baud, slave),
    connectVdc32Tcp: async (ip, port, slave, timeout) => await _deviceService.ConnectTcpAsync(ip, port, slave, timeout),
    connectLoadDevice: (port, baud) => _loadService.Connect(port, baud),
    disconnectVdc32: async () => await DisconnectVdc32Async(),
    disconnectLoadDevice: async () => await DisconnectLoadDeviceAsync(),
    isVdc32Connected: () => _deviceService.IsConnected,
    isLoadDeviceConnected: () => _loadService.IsConnected
);

// 订阅事件
coordinator.ConnectionStateChanged += (s, e) => {
    UpdateUI(e.DeviceType, e.IsConnected);
};

// 连接设备 (自动处理互斥)
await coordinator.ConnectVdc32SerialAsync("COM3", 9600, 1);
```

### PollingCoordinator

```csharp
// 创建轮询协调器
var polling = new PollingCoordinator(
    vdc32PollingAction: async (token) => await ReadVdc32DataAsync(token),
    loadDevicePollingAction: async (token) => await ReadLoadDataAsync(token)
);

// 订阅事件
polling.PollingCycleCompleted += (s, e) => {
    Debug.WriteLine($"周期 {e.CycleNumber}: {e.ElapsedMs}ms");
};

// 启动轮询
await polling.StartVdc32PollingAsync();

// 暂停执行配置操作
await polling.PauseAndExecuteAsync(async () => {
    await ConfigureDeviceAsync();
});
```

### UIStateCoordinator

```csharp
// 创建 UI 状态协调器
var uiState = new UIStateCoordinator(mainView);

// 更新连接状态
uiState.UpdateConnectionState(DeviceType.VDC32, isConnected: true);

// 视图切换
uiState.PrepareViewSwitch("LOAD");
// ... 执行切换 ...
uiState.CompleteViewSwitch("LOAD");
```

## 依赖关系

```
                    +------------------+
                    | MainPresenter    |
                    +--------+---------+
                             |
         +-------------------+-------------------+
         |                   |                   |
         v                   v                   v
+--------+--------+ +--------+--------+ +--------+--------+
|DeviceConnection | |Polling          | |UIState          |
|Coordinator      | |Coordinator      | |Coordinator      |
+-----------------+ +-----------------+ +-----------------+
         |                   |                   |
         v                   v                   v
   DeviceService      CancellationToken    IMainView
   LoadDeviceService                       IVdc32View
                                          ILoadDeviceView
```

## 待办事项

- [ ] 在 MainForm 中实例化协调器
- [ ] 将 btnConnect_Click 逻辑迁移到 DeviceConnectionCoordinator
- [ ] 将 StartContinuousPollingAsync 逻辑迁移到 PollingCoordinator
- [ ] 将 UpdateUIState 逻辑迁移到 UIStateCoordinator
