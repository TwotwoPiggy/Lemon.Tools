# PDF转Word工具类使用说明

## 概述

CommonTools.Pdf命名空间提供了PDF转Word文档的转换功能,基于Spire.PDF库实现。支持单文件转换和批量转换,提供可插拔的转换器架构。

## 安装

### 1. 安装NuGet包

```bash
dotnet add package Spire.PDF
```

### 2. 添加项目引用

```xml
<ProjectReference Include="..\CommonTools\CommonTools.csproj" />
```

## 快速开始

### 基本用法

#### 1. 单文件转换

```csharp
using CommonTools.Pdf;

// 最简单的用法
var result = await PdfConverter.ConvertAsync("input.pdf", "output.docx");

if (result.Success)
{
    Console.WriteLine($"转换成功! 输出文件: {result.OutputPath}");
    Console.WriteLine($"耗时: {result.DurationMilliseconds} 毫秒");
    Console.WriteLine($"页数: {result.PageCount}");
}
else
{
    Console.WriteLine($"转换失败: {result.ErrorMessage}");
}
```

#### 2. 使用自定义选项

```csharp
// 使用高质量转换选项
var options = ConversionOptions.HighQuality();
var result = await PdfConverter.ConvertAsync("input.pdf", "output.docx", options);

// 或使用可编辑转换选项
var editableOptions = ConversionOptions.Editable();
var result = await PdfConverter.ConvertAsync("input.pdf", "output.docx", editableOptions);

// 或自定义选项
var customOptions = new ConversionOptions
{
    UseFixedLayout = true,
    OutputFormat = WordFormat.Docx,
    ExtractImages = true,
    KeepOriginalFonts = true,
    RecognizeTables = true
};
var result = await PdfConverter.ConvertAsync("input.pdf", "output.docx", customOptions);
```

#### 3. 批量转换

```csharp
var pdfFiles = new[] { "file1.pdf", "file2.pdf", "file3.pdf" };
var outputFolder = @"C:\Output";

// 带进度回调
var progress = new Progress<int>(percent => 
{
    Console.WriteLine($"转换进度: {percent}%");
});

var result = await PdfConverter.ConvertBatchAsync(pdfFiles, outputFolder, progress: progress);

Console.WriteLine(result.GetSummary());
// 输出: 批量转换完成: 总计 3 个文件, 成功 3 个, 失败 0 个, 耗时 XXX 毫秒

// 查看失败的转换
if (!result.AllSuccess)
{
    foreach (var failed in result.FailedResults)
    {
        Console.WriteLine($"失败文件: {failed.SourcePath}, 错误: {failed.ErrorMessage}");
    }
}
```

#### 4. 使用取消令牌

```csharp
var cts = new CancellationTokenSource();

// 设置超时
cts.CancelAfter(TimeSpan.FromMinutes(5));

try
{
    var result = await PdfConverter.ConvertAsync("input.pdf", "output.docx", cancellationToken: cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("转换操作已取消");
}
```

## 高级用法

### 1. 使用自定义转换器

```csharp
// 创建自定义转换器实例
var converter = new SpirePdfConverter();

// 使用自定义转换器
var result = await PdfConverter.ConvertAsync(converter, "input.pdf", "output.docx");

// 或直接使用转换器
var result = await converter.ConvertAsync("input.pdf", "output.docx");
```

### 2. 实现自定义转换器

```csharp
public class CustomPdfConverter : IPdfToWordConverter
{
    public string ConverterInfo => "Custom PDF Converter v1.0";

    public async Task<ConversionResult> ConvertAsync(
        string pdfPath, 
        string wordPath, 
        ConversionOptions? options = null, 
        CancellationToken cancellationToken = default)
    {
        // 实现自定义转换逻辑
        // ...
    }

    public async Task<BatchConversionResult> ConvertBatchAsync(
        IEnumerable<string> pdfPaths,
        string outputFolder,
        ConversionOptions? options = null,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        // 实现批量转换逻辑
        // ...
    }
}
```

## 转换选项说明

### ConversionOptions

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| UseFixedLayout | bool | true | 是否使用固定布局模式(保持PDF原始布局) |
| OutputFormat | WordFormat | Docx | 输出格式(Doc或Docx) |
| ExtractImages | bool | true | 是否提取PDF中的图片 |
| KeepOriginalFonts | bool | true | 是否保留PDF的原始字体 |
| RecognizeTables | bool | true | 是否识别表格 |
| RemoveEmptyLines | bool | false | 是否删除空白行 |
| TimeoutSeconds | int | 300 | 转换超时时间(秒) |
| RetryOnFailure | bool | false | 是否在转换失败时重试 |
| RetryCount | int | 3 | 重试次数 |

### 预设选项

#### Default()
默认转换选项,适用于大多数场景。

#### HighQuality()
高质量转换选项,固定布局,保留所有格式,适合复杂格式的文档。

#### Editable()
可编辑转换选项,流式布局,便于编辑,适合需要修改内容的文档。

## 异常处理

### PdfConversionException

PDF转换过程中发生的异常,包含以下属性:
- `SourcePdfPath`: 源PDF文件路径
- `TargetWordPath`: 目标Word文件路径

### 常见异常

1. **FileNotFoundException**: PDF文件不存在
2. **ArgumentNullException**: 参数为null或空
3. **DirectoryNotFoundException**: 输出文件夹不存在
4. **PdfConversionException**: 转换过程中发生错误

### 异常处理示例

```csharp
try
{
    var result = await PdfConverter.ConvertAsync("input.pdf", "output.docx");
    
    if (!result.Success)
    {
        Console.WriteLine($"转换失败: {result.ErrorMessage}");
        if (result.Exception != null)
        {
            Console.WriteLine($"异常详情: {result.Exception.Message}");
        }
    }
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"文件不存在: {ex.FileName}");
}
catch (PdfConversionException ex)
{
    Console.WriteLine($"PDF转换异常: {ex.Message}");
    Console.WriteLine($"源文件: {ex.SourcePdfPath}");
    Console.WriteLine($"目标文件: {ex.TargetWordPath}");
}
catch (Exception ex)
{
    Console.WriteLine($"未知异常: {ex.Message}");
}
```

## 注意事项

### Spire.PDF免费版限制

- 每个PDF文档最多支持10页
- 超过限制需要购买商业许可证

### 性能建议

1. 对于大文件,建议使用批量转换并设置合理的超时时间
2. 使用进度回调监控转换进度
3. 在服务器环境使用时,注意资源释放和并发控制

### 最佳实践

1. 始终检查转换结果的Success属性
2. 使用using语句或正确释放资源
3. 处理所有可能的异常情况
4. 对于批量转换,检查FailedResults获取失败详情

## 完整示例

```csharp
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommonTools.Pdf;

class Program
{
    static async Task Main(string[] args)
    {
        var pdfPath = "sample.pdf";
        var outputPath = "sample.docx";

        // 检查文件是否存在
        if (!File.Exists(pdfPath))
        {
            Console.WriteLine($"文件不存在: {pdfPath}");
            return;
        }

        try
        {
            // 创建取消令牌(5分钟超时)
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));

            // 使用高质量转换选项
            var options = ConversionOptions.HighQuality();

            // 执行转换
            var result = await PdfConverter.ConvertAsync(
                pdfPath, 
                outputPath, 
                options, 
                cts.Token);

            // 检查结果
            if (result.Success)
            {
                Console.WriteLine("转换成功!");
                Console.WriteLine($"输出文件: {result.OutputPath}");
                Console.WriteLine($"页数: {result.PageCount}");
                Console.WriteLine($"耗时: {result.DurationMilliseconds} 毫秒");
            }
            else
            {
                Console.WriteLine($"转换失败: {result.ErrorMessage}");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("转换操作超时已取消");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生异常: {ex.Message}");
        }
    }
}
```

## 更多资源

- [Spire.PDF官方文档](https://www.e-iceblue.com/Introduce/pdf-for-net-introduce.html)
- [项目GitHub仓库](https://github.com/your-repo/CommonTools)
- [问题反馈](https://github.com/your-repo/CommonTools/issues)
