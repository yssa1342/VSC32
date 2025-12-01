# Exporters/ - 数据导出器层

## 概述

Exporters 层负责将设备数据导出为各种格式（CSV、文本等），遵循单一职责原则。

## 包含文件

### DataExporter.cs (~180 行)

数据导出器，实现 `IDataExporter` 接口。

**职责：**
- VDC-32 电压数据导出 (CSV)
- GJDD-750 负载数据导出 (CSV)
- 运行日志导出 (TXT)
- 生成带时间戳的文件名

**公共方法：**
- `ExportVdc32ToCsvAsync()` - 导出电压数据
- `ExportLoadDeviceToCsvAsync()` - 导出负载数据
- `ExportLogAsync()` - 导出日志
- `GenerateFileName()` - 生成文件名

## 使用示例

```csharp
var exporter = new DataExporter();

// 导出 VDC-32 数据
double[] voltages = /* 从设备读取 */;
bool[] alarms = /* 报警状态 */;

var result = await exporter.ExportVdc32ToCsvAsync(
    "C:/Export/vdc32_data.csv",
    voltages,
    alarms,
    "VDC-32 #001"
);

if (result.Success)
{
    MessageBox.Show($"已导出 {result.RecordCount} 条记录到 {result.FilePath}");
}
else
{
    MessageBox.Show($"导出失败: {result.ErrorMessage}");
}
```

## 导出格式

### VDC-32 CSV 格式
```csv
# VDC-32 电压数据导出
# 导出时间: 2025-12-01 10:30:00
# 设备信息: VDC-32 #001

通道,电压(V),状态
CH01,24.123,正常
CH02,24.056,正常
CH03,23.890,报警
...
```

### GJDD-750 CSV 格式
```csv
# GJDD-750 负载数据导出
# 导出时间: 2025-12-01 10:30:00
# 设备信息: GJDD-750 #001

通道,电流(A),电压(V),功率(W),状态
CH1,30.50,24.00,732.0,开启
CH2,0.00,24.00,0.0,关闭
...
```

## 扩展

如需添加新的导出格式（如 Excel、JSON），可：
1. 在 `ExportFormat` 枚举中添加格式
2. 实现对应的导出方法
3. 必要时使用第三方库（如 EPPlus 用于 Excel）
