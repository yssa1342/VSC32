# Views 视图控件层

## 概述

Views 层包含可复用的 WinForms 用户控件，实现相应的视图接口。每个控件负责特定的 UI 区域，与业务逻辑解耦。

## 文件列表

| 文件 | 职责 | 代码行数 |
|------|------|----------|
| `Vdc32ChannelView.cs` | VDC-32 通道显示 | ~260 行 |
| `LoadDeviceView.cs` | GJDD-750 负载控制 | ~350 行 |
| `ConnectionPanelView.cs` | 连接配置面板 | ~310 行 |
| `LogView.cs` | 运行日志显示 | ~230 行 |

## 架构规则遵从

✅ 每个类 < 500 行  
✅ 实现视图接口 (IVdc32View, ILoadDeviceView, IConnectionPanel)  
✅ 只负责 UI 显示和事件触发  
✅ 不包含业务逻辑  
✅ 使用 InvokeIfRequired 确保线程安全  

## 使用示例

### 在 MainForm 中使用

```csharp
public partial class MainForm : Form, IMainView
{
    private Vdc32ChannelView _vdc32View;
    private LoadDeviceView _loadView;
    private ConnectionPanelView _connectionPanel;
    private LogView _logView;

    public MainForm()
    {
        InitializeComponent();
        
        // 创建视图控件
        _vdc32View = new Vdc32ChannelView
        {
            Dock = DockStyle.Fill
        };
        panelContent.Controls.Add(_vdc32View);

        _loadView = new LoadDeviceView
        {
            Dock = DockStyle.Fill,
            Visible = false
        };
        panelContent.Controls.Add(_loadView);

        _connectionPanel = new ConnectionPanelView
        {
            Dock = DockStyle.Top
        };
        panelRight.Controls.Add(_connectionPanel);

        _logView = new LogView
        {
            Dock = DockStyle.Fill
        };
        panelLog.Controls.Add(_logView);
    }

    // IMainView 实现
    public IVdc32View Vdc32View => _vdc32View;
    public ILoadDeviceView LoadDeviceView => _loadView;
    public IConnectionPanel ConnectionPanel => _connectionPanel;

    public void AddLog(string message, bool? success)
    {
        _logView.AddLog(message, success);
    }
}
```

### 事件绑定

```csharp
// 连接事件
_connectionPanel.ConnectRequested += async (s, e) =>
{
    await _presenter.HandleConnectClickAsync();
};

// 负载设备通道切换
_loadView.ChannelToggleRequested += async (s, channelIndex) =>
{
    await _loadPresenter.HandleChannelToggleAsync(channelIndex);
};

// 日志导出
_logView.ExportRequested += (s, e) =>
{
    SaveFileDialog dialog = new SaveFileDialog
    {
        Filter = "文本文件|*.txt|所有文件|*.*",
        FileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
    };
    
    if (dialog.ShowDialog() == DialogResult.OK)
    {
        File.WriteAllText(dialog.FileName, _logView.GetAllLogsText());
    }
};
```

## 控件职责

### Vdc32ChannelView

- 显示 32 通道电压值
- 显示状态指示器（正常/报警/离线）
- 显示设备信息（固件版本、设备名称、从机地址）
- 显示环境数据（温度、湿度）
- 显示 IO 状态

### LoadDeviceView

- 显示 8 通道负载数据（电流、功率）
- 提供通道开关控制按钮
- 单通道电流配置
- 批量操作（全部开启/关闭/设置电流）
- 显示变频器状态

### ConnectionPanelView

- 串口/TCP 连接方式切换
- 串口选择和波特率配置
- TCP IP 和端口配置
- 从机地址配置
- 连接状态显示
- 连接/断开按钮

### LogView

- 实时日志显示（带颜色区分）
- 自动滚动开关
- 日志计数
- 清空和导出功能

## 设计原则

### 1. 单一职责

每个控件只负责一个 UI 区域的显示和交互。

### 2. 接口隔离

通过实现特定接口，控件可以独立测试和替换。

### 3. 线程安全

所有 UI 更新通过 `InvokeIfRequired` 确保在 UI 线程执行：

```csharp
private void InvokeIfRequired(Action action)
{
    if (InvokeRequired)
        Invoke(action);
    else
        action();
}
```

### 4. 事件驱动

用户操作通过事件通知 Presenter，而不是直接执行业务逻辑：

```csharp
// 控件只触发事件
_btnConnect.Click += (s, e) => ConnectRequested?.Invoke(this, EventArgs.Empty);

// Presenter 处理业务逻辑
connectionPanel.ConnectRequested += async (s, e) =>
{
    await _presenter.HandleConnectClickAsync();
};
```

## 待办事项

- [ ] 将控件添加到 MainForm
- [ ] 绑定控件事件到 Presenter
- [ ] 添加设计器支持 (.Designer.cs)
- [ ] 添加自定义控件（StatusIndicator, ToggleSwitch 等）
