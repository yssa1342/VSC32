# Presenters 展示器层

## 概述

Presenter 是 MVP 模式的核心，负责协调 View（视图）和 Model（业务逻辑）。Presenter 不直接操作 UI 控件，而是通过接口与 View 交互。

## 文件列表

| 文件 | 职责 | 代码行数 |
|------|------|----------|
| `MainPresenter.cs` | 主窗体协调器 | ~280 行 |
| `Vdc32Presenter.cs` | VDC-32 业务逻辑 | ~200 行 |
| `LoadDevicePresenter.cs` | GJDD-750 业务逻辑 | ~240 行 |

## 架构规则遵从

✅ 每个类 < 500 行  
✅ 每个方法 < 50 行  
✅ 参数对象封装配置 (XxxConfig)  
✅ 通过接口与 View 交互  
✅ 不直接操作 UI 控件  
✅ 实现 IDisposable 正确释放资源  

## MVP 模式架构

```
┌─────────────────────────────────────────────────────────────┐
│                         MainForm                             │
│  (implements IMainView, IVdc32View, ILoadDeviceView)        │
└─────────────────────────────────────────────────────────────┘
                              │
                              │ 持有引用
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                       MainPresenter                          │
│  ─────────────────────────────────────────────────────────  │
│  + HandleConnectClickAsync()                                 │
│  + HandleViewSwitchAsync()                                   │
│  + HandleFormClosingAsync()                                  │
└──────────────────────┬──────────────────────────────────────┘
                       │
          ┌────────────┴────────────┐
          ▼                         ▼
┌──────────────────┐     ┌──────────────────────┐
│  Vdc32Presenter  │     │  LoadDevicePresenter │
│  ──────────────  │     │  ──────────────────  │
│  + ReadDataOnce  │     │  + HandleToggle      │
│  + UpdateDevice  │     │  + HandleSetCurrent  │
│  + ResetDisplay  │     │  + HandleBatchOps    │
└──────────────────┘     └──────────────────────┘
```

## 使用示例

### MainPresenter 初始化

```csharp
// 在 MainForm 构造函数中
_presenter = new MainPresenter(new MainPresenterConfig
{
    View = this,  // MainForm 实现 IMainView
    ConnectionCoordinator = _connectionCoordinator,
    PollingCoordinator = _pollingCoordinator,
    UIStateCoordinator = _uiStateCoordinator,
    ViewSwitchHandler = _viewSwitchHandler,
    Vdc32Presenter = _vdc32Presenter,
    LoadPresenter = _loadPresenter
});

_presenter.Initialize();
```

### 事件处理委托给 Presenter

```csharp
// 连接按钮点击
private async void btnConnect_Click(object sender, EventArgs e)
{
    await _presenter.HandleConnectClickAsync();
}

// 视图切换
private async void btnMenuVdc32_Click(object sender, EventArgs e)
{
    await _presenter.HandleViewSwitchAsync("VDC32");
}

// 窗体关闭
private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
{
    e.Cancel = true;
    await _presenter.HandleFormClosingAsync();
    e.Cancel = false;
}
```

### 子 Presenter 使用

```csharp
// Vdc32Presenter
_vdc32Presenter = new Vdc32Presenter(new Vdc32PresenterConfig
{
    View = this,
    PollingCoordinator = _pollingCoordinator,
    DataReadHandler = _dataReadHandler,
    DisplayHandler = _displayHandler,
    LogAction = AddLog
});

// LoadDevicePresenter
_loadPresenter = new LoadDevicePresenter(new LoadDevicePresenterConfig
{
    View = this,
    PollingCoordinator = _pollingCoordinator,
    DataReadHandler = _dataReadHandler,
    ChannelHandler = _channelHandler,
    DisplayHandler = _displayHandler,
    LogAction = AddLog
});
```

## 设计原则

### 1. 关注点分离

- **View (MainForm):** 只负责 UI 更新和用户输入捕获
- **Presenter:** 协调业务逻辑，决定何时更新 View
- **Model (Handlers/Coordinators):** 具体业务实现

### 2. 单向数据流

```
User Action → View → Presenter → Model
                         ↓
              View ← Presenter ← Model
```

### 3. 可测试性

Presenter 可以独立进行单元测试：

```csharp
[TestMethod]
public async Task HandleConnect_ShouldStartPolling_WhenConnectionSucceeds()
{
    // Arrange
    var mockView = new Mock<IMainView>();
    var mockCoordinator = new Mock<IDeviceConnectionCoordinator>();
    mockCoordinator.Setup(c => c.ConnectVdc32SerialAsync(...)).ReturnsAsync(true);
    
    var presenter = new MainPresenter(new MainPresenterConfig { ... });
    
    // Act
    await presenter.HandleConnectClickAsync();
    
    // Assert
    mockPollingCoordinator.Verify(p => p.StartVdc32PollingAsync(), Times.Once);
}
```

## 待办事项

- [ ] MainForm 实现 IMainView, IVdc32View, ILoadDeviceView 接口
- [ ] 将 MainForm 中的事件处理全部委托给 MainPresenter
- [ ] 移除 MainForm 中的业务逻辑代码
- [ ] 添加 Presenter 单元测试
