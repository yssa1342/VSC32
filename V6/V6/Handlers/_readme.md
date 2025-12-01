# Handlers 处理器层

## 概述

处理器负责处理特定的业务事件和操作逻辑，是 Presenter 和具体操作之间的桥梁。

## 文件列表

| 文件 | 职责 | 代码行数 |
|------|------|----------|
| `ConnectionHandler.cs` | 设备连接/断开业务逻辑 | ~220 行 |
| `DataReadHandler.cs` | 设备数据读取和转换 | ~230 行 |
| `ViewSwitchHandler.cs` | 主界面视图切换 | ~170 行 |
| `LoadChannelHandler.cs` | 负载通道控制操作 | ~270 行 |
| `ChannelDisplayHandler.cs` | 通道数据显示更新 | ~230 行 |

## 架构规则遵从

✅ 每个类 < 500 行  
✅ 每个方法 < 50 行  
✅ 方法参数 ≤ 5 个  
✅ 使用常量定义所有 Magic Numbers  
✅ Result 模式封装操作结果  
✅ 通过委托依赖注入底层服务  

## 使用示例

### ConnectionHandler

```csharp
var handler = new ConnectionHandler(
    connectionCoordinator,
    pollingCoordinator,
    uiStateCoordinator,
    (msg, success) => AddLog(msg, success)
);

var result = await handler.HandleVdc32SerialConnectAsync("COM3", 9600, 1);
if (!result.Success)
{
    MessageBox.Show(result.Message);
}
```

### DataReadHandler

```csharp
var handler = new DataReadHandler(
    async (token) => await _deviceService.ReadHoldingRegistersAsync(0, 32, token),
    async (token) => await _loadService.ReadAllChannelsAsync(token),
    (msg, success) => AddLog(msg, success)
);

var result = await handler.ReadVdc32ChannelsAsync(cancellationToken);
if (result.Success)
{
    _displayHandler.UpdateVdc32Channels(result.Voltages, result.Alarms);
}
```

### ViewSwitchHandler

```csharp
var handler = new ViewSwitchHandler(new ViewSwitchConfig
{
    Vdc32Panel = panelVdc32,
    LoadPanel = panelLoad,
    LogPanel = panelLog,
    ConnectionPanel = panelConnection,
    Vdc32MenuButton = btnMenuVdc32,
    LoadMenuButton = btnMenuLoad,
    LogMenuButton = btnMenuLog,
    OnViewChanged = (view) => UpdateConnectionPanelForView(view)
});

handler.SwitchToLoad();
```

### LoadChannelHandler

```csharp
var handler = new LoadChannelHandler(
    async (ch, state) => await _loadService.SetChannelStateAsync(ch, state),
    async (ch, current) => await _loadService.SetChannelCurrentAsync(ch, current),
    async (current) => await _loadService.SetAllCurrentAsync(current),
    async (state) => await _loadService.SetAllStateAsync(state),
    (msg, success) => AddLog(msg, success)
);

await handler.ToggleChannelAsync(0);
await handler.SetAllChannelsCurrentAsync(30.0);
```

### ChannelDisplayHandler

```csharp
var displayHandler = new ChannelDisplayHandler(this);
displayHandler.ConfigureVdc32Display(voltageLabels, indicatorPanels);
displayHandler.ConfigureLoadDisplay(currentLabels, powerLabels, loadIndicators, toggleButtons);

// 在轮询回调中
displayHandler.UpdateVdc32Channels(voltages, alarms);
```

## 设计模式

### Result Pattern

```
┌─────────────────────┐
│  Handler Method     │
│  ─────────────────  │
│  async Task<Result> │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  XxxResult          │
│  ─────────────────  │
│  + Success: bool    │
│  + Message: string  │
│  + Data (optional)  │
└─────────────────────┘
```

### 优势

1. **统一错误处理**：Result 模式避免到处 try-catch
2. **可测试性**：Handler 可独立单元测试
3. **SRP 遵从**：每个 Handler 专注一个职责
4. **线程安全**：InvokeIfRequired 处理 UI 更新

## 依赖关系

```
┌──────────────┐     ┌──────────────────────┐
│  Presenter   │────▶│  Handlers            │
└──────────────┘     │  ──────────────────  │
                     │  ConnectionHandler   │
                     │  DataReadHandler     │
                     │  LoadChannelHandler  │
                     │  ChannelDisplayHandler│
                     │  ViewSwitchHandler   │
                     └──────────┬───────────┘
                                │
                     ┌──────────▼───────────┐
                     │  Coordinators/Services│
                     └──────────────────────┘
```

## 待办事项

- [ ] 在 MainPresenter 中实例化各 Handler
- [ ] 将 MainForm 的事件处理逻辑迁移到 Handler
- [ ] 添加 DeviceInfoHandler（固件版本读取）
- [ ] 添加 IoStatusHandler（IO 状态处理）
