# Builders 构建器层

## 概述

构建器负责创建复杂的 UI 组件，使用 Builder 模式支持链式配置，将 UI 构建逻辑从 Form 中提取出来。

## 文件列表

| 文件 | 职责 | 代码行数 |
|------|------|----------|
| `ChannelPanelBuilder.cs` | VDC-32 的 32 通道显示面板 | ~210 行 |
| `LoadChannelPanelBuilder.cs` | GJDD-750 的 8 通道负载控制面板 | ~260 行 |
| `ConnectionPanelBuilder.cs` | 串口/TCP 连接配置面板 | ~340 行 |
| `MenuBuilder.cs` | 左侧导航菜单 | ~160 行 |
| `StatusBarBuilder.cs` | 底部状态栏 | ~130 行 |

## 架构规则遵从

✅ 每个类 < 500 行  
✅ 每个方法 < 50 行  
✅ Builder 模式支持链式调用  
✅ 控件创建与业务逻辑分离  
✅ 使用常量定义 UI 尺寸和颜色  

## 使用示例

### ChannelPanelBuilder

```csharp
var result = new ChannelPanelBuilder(channelContainer)
    .WithNormalColor(Color.Green)
    .WithAlarmColor(Color.Red)
    .WithVoltageFont(new Font("Segoe UI", 14f, FontStyle.Bold))
    .Build();

if (result.Success)
{
    _voltageLabels = result.VoltageLabels;
    _indicatorPanels = result.IndicatorPanels;
}
```

### LoadChannelPanelBuilder

```csharp
var result = new LoadChannelPanelBuilder(loadContainer)
    .WithOnColor(Color.LimeGreen)
    .WithOffColor(Color.Gray)
    .Build();

// 绑定按钮事件
foreach (var btn in result.ToggleButtons)
{
    btn.Click += OnChannelToggleClick;
}
```

### ConnectionPanelBuilder

```csharp
var result = new ConnectionPanelBuilder(connectionPanel)
    .WithTitle("VDC-32 连接配置")
    .WithTcpOption(true)
    .WithSlaveIdOption(true)
    .WithAccentColor(Color.FromArgb(33, 150, 243))
    .Build();

result.ConnectButton.Click += OnConnectClick;
```

### MenuBuilder

```csharp
var result = new MenuBuilder(menuPanel)
    .AddItem("VDC32", "VDC-32 电压检测", "📊")
    .AddItem("LOAD", "GJDD-750 负载控制", "⚡")
    .AddItem("LOG", "运行日志", "📋")
    .WithActiveBackColor(Color.FromArgb(62, 62, 66))
    .Build();

foreach (var kvp in result.MenuButtons)
{
    kvp.Value.Click += OnMenuClick;
}
```

## 设计模式

### Builder Pattern

```
┌─────────────────┐      ┌──────────────────┐
│  Client Code    │──────│  XxxBuilder      │
│  (MainForm)     │      │  ─────────────── │
└─────────────────┘      │  + WithXxx()     │
                         │  + Build()       │
                         └────────┬─────────┘
                                  │
                                  ▼
                         ┌──────────────────┐
                         │  XxxBuildResult  │
                         │  ─────────────── │
                         │  + Success       │
                         │  + Controls[]    │
                         └──────────────────┘
```

### 优势

1. **可读性**：链式调用清晰表达配置意图
2. **可测试**：Builder 可独立单元测试
3. **可复用**：相同 Builder 可用于不同场景
4. **SRP 遵从**：Form 不再负责 UI 构建细节

## 待办事项

- [ ] 在 MainForm.InitializeComponent 后调用 Builders
- [ ] 将现有的手动控件创建代码迁移到 Builders
- [ ] 添加 DeviceInfoPanelBuilder（固件版本、设备名称等）
- [ ] 添加 IoStatusPanelBuilder（IO 状态显示）
