# Twotwo.Agent

基于 Google Gemini API 的 AI Agent 服务库，支持依赖注入，便于在 .NET 应用中集成 Gemini AI 能力。

## 功能特性

- 支持文本和多模态（图片）内容生成
- 原生支持依赖注入（DI）
- 可配置代理
- 内置 Prompt 管理器
- 模型有效性验证

## 安装

通过 NuGet 安装：

```bash
dotnet add package Twotwo.Agent
```

## 快速开始

### 1. 配置 appsettings.json

```json
{
  "AIConfig": {
    "ApiKey": "your-gemini-api-key",
    "ModelName": "gemini-2.5-flash",
    "RPM": 5,
    "TPM": 250000,
    "RPD": 20,
    "Proxy": {
      "Enabled": true,
      "Address": "http://127.0.0.1:10808"
    }
  }
}
```

### 2. 注册服务

```csharp
using Microsoft.Extensions.DependencyInjection;
using Twotwo.Agent.Extensions;

// 方式1: 从配置文件读取
builder.Services.AddGeminiAgent(builder.Configuration, "AIConfig");

// 方式2: 直接传入配置对象
builder.Services.AddGeminiAgent(new AIConfig
{
    ApiKey = "your-api-key",
    ModelName = "gemini-2.5-flash"
});

// 方式3: 使用 Action 委托配置
builder.Services.AddGeminiAgent(config =>
{
    config.ApiKey = "your-api-key";
    config.ModelName = "gemini-2.5-flash";
});
```

### 3. 使用服务

```csharp
using Twotwo.Agent.Interfaces;
using Twotwo.Agent.Services;
using Twotwo.Agent.Types;

public class MyService
{
    private readonly IGeminiAgentService _agent;

    public MyService(IGeminiAgentService agent)
    {
        _agent = agent;
    }

    public async Task<string> GenerateTextAsync(string prompt)
    {
        var response = await _agent.GenerateContentAsync(new AIRequest(prompt));
        return response.Text;
    }
}
```

## API 参考

### AIConfig 配置项

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| ApiKey | string | - | Gemini API 密钥（必填） |
| ModelName | string | gemini-2.5-flash | 模型名称 |
| RPM | int | 5 | 每分钟请求数限制 |
| TPM | long | 250000 | 每分钟 Token 数限制 |
| RPD | int | 20 | 每日请求数限制 |
| Proxy | ProxyConfig | - | 代理配置 |

### AIRequest 请求参数

| 属性 | 类型 | 说明 |
|------|------|------|
| Text | string | 提示文本 |
| Files | List\<byte[]> | 文件字节数组列表（可选） |
| MimeType | string | 文件 MIME 类型（可选） |
| ModelName | string | 指定模型名称（可选，覆盖默认配置） |

### GeminiResponse 响应

| 属性 | 类型 | 说明 |
|------|------|------|
| Text | string | 生成的文本内容 |
| Candidates | List\<Candidate> | 候选结果列表 |
| ModelVersion | string | 模型版本 |
| UsageMetadata | GenerateContentResponseUsageMetadata | Token 使用统计 |
| Parts | List\<Part> | 内容片段 |

## 多模态示例（图片识别）

```csharp
using Twotwo.Agent.Constants;
using Twotwo.Agent.Services;

// 加载 Prompt 配置
PromptLoader.Load("Prompts.json");

// 读取图片
var imageBytes = File.ReadAllBytes("image.jpg");

var request = new AIRequest(
    Text: PromptLoader.Get(PromptConstants.OcrAssistantPrompt),
    Files: new List<byte[]> { imageBytes },
    MimeType: "image/jpeg"
);

var response = await _agent.GenerateContentAsync(request);
Console.WriteLine(response.Text);
```

## Prompt 管理

项目内置 `PromptLoader` 用于管理 Prompt 模板：

```json
// Prompts.json
{
  "OcrAssistantPrompt": "提取图片购物信息返回JSON...",
  "SummaryPrompt": "请对给定文本做简短摘要。"
}
```

使用方式：

```csharp
PromptLoader.Load("Prompts.json");
var prompt = PromptLoader.Get("OcrAssistantPrompt");
```

## 模型验证

可选地验证模型是否可用：

```csharp
var isValid = await _agent.ValidateModelAsync();
```

## 依赖项

- Google.GenAI (>= 1.3.0)
- Microsoft.Extensions.DependencyInjection.Abstractions (>= 9.0.0)
- Microsoft.Extensions.Options (>= 9.0.0)
- Polly (>= 8.6.6)

## 许可证

MIT License
