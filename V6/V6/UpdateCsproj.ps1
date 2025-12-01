# UpdateCsproj.ps1
# 自动将新文件添加到 V6.csproj

$csprojPath = "V6.csproj"

# 读取 csproj 文件
$content = Get-Content $csprojPath -Raw

# 定义要添加的新文件条目
$newItems = @"
    <!-- ===================== MVP 架构文件 ===================== -->
    <!-- Interfaces -->
    <Compile Include="Interfaces\IMainView.cs" />
    <Compile Include="Interfaces\IVdc32View.cs" />
    <Compile Include="Interfaces\ILoadDeviceView.cs" />
    <Compile Include="Interfaces\IConnectionPanel.cs" />
    <Compile Include="Interfaces\IDeviceConnectionCoordinator.cs" />
    <Compile Include="Interfaces\IPollingCoordinator.cs" />
    <Compile Include="Interfaces\IUIStateCoordinator.cs" />
    <Compile Include="Interfaces\IDataExporter.cs" />
    <Compile Include="Interfaces\EventArgs.cs" />
    <!-- Coordinators -->
    <Compile Include="Coordinators\DeviceConnectionCoordinator.cs" />
    <Compile Include="Coordinators\PollingCoordinator.cs" />
    <Compile Include="Coordinators\UIStateCoordinator.cs" />
    <!-- Builders -->
    <Compile Include="Builders\ChannelPanelBuilder.cs" />
    <Compile Include="Builders\LoadChannelPanelBuilder.cs" />
    <Compile Include="Builders\ConnectionPanelBuilder.cs" />
    <Compile Include="Builders\MenuBuilder.cs" />
    <Compile Include="Builders\StatusBarBuilder.cs" />
    <!-- Handlers -->
    <Compile Include="Handlers\ConnectionHandler.cs" />
    <Compile Include="Handlers\DataReadHandler.cs" />
    <Compile Include="Handlers\ViewSwitchHandler.cs" />
    <Compile Include="Handlers\LoadChannelHandler.cs" />
    <Compile Include="Handlers\ChannelDisplayHandler.cs" />
    <!-- Presenters -->
    <Compile Include="Presenters\MainPresenter.cs" />
    <Compile Include="Presenters\Vdc32Presenter.cs" />
    <Compile Include="Presenters\LoadDevicePresenter.cs" />
    <!-- Views -->
    <Compile Include="Views\Vdc32ChannelView.cs">
      <SubType>UserControl</SubType>
    </Compile>
    <Compile Include="Views\LoadDeviceView.cs">
      <SubType>UserControl</SubType>
    </Compile>
    <Compile Include="Views\ConnectionPanelView.cs">
      <SubType>UserControl</SubType>
    </Compile>
    <Compile Include="Views\LogView.cs">
      <SubType>UserControl</SubType>
    </Compile>
    <!-- Exporters -->
    <Compile Include="Exporters\DataExporter.cs" />
    <!-- Forms -->
    <Compile Include="Forms\MainForm.Refactored.cs">
      <SubType>Form</SubType>
    </Compile>
    <!-- ===================== END MVP 架构文件 ===================== -->
"@

# 查找第一个 <Compile Include="MainForm.cs" 并在其前面插入新内容
$pattern = '(\s*<Compile Include="MainForm\.cs")'
$replacement = "$newItems`r`n`$1"

$newContent = $content -replace $pattern, $replacement

# 检查是否有变化
if ($content -eq $newContent) {
    Write-Host "警告: 未能找到插入点，尝试其他方式..." -ForegroundColor Yellow
    
    # 尝试另一种方式：在 <ItemGroup> 后面的第一个 <Compile 前插入
    $pattern2 = '(<ItemGroup>\s*)(<Compile)'
    $replacement2 = "`$1$newItems`r`n    `$2"
    $newContent = $content -replace $pattern2, $replacement2, 1
}

if ($content -ne $newContent) {
    # 备份原文件
    Copy-Item $csprojPath "$csprojPath.backup"
    Write-Host "已备份原文件到 $csprojPath.backup" -ForegroundColor Green
    
    # 写入新内容
    Set-Content $csprojPath -Value $newContent -NoNewline
    Write-Host "已成功更新 $csprojPath" -ForegroundColor Green
    Write-Host "添加了 31 个新文件引用" -ForegroundColor Cyan
} else {
    Write-Host "错误: 无法更新 csproj 文件，请手动添加文件引用" -ForegroundColor Red
    Write-Host ""
    Write-Host "请在 V6.csproj 的 <ItemGroup> 中添加以下内容:" -ForegroundColor Yellow
    Write-Host $newItems
}
