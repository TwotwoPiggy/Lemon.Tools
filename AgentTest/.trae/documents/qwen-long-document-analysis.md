# Qwen-Long 文档分析实现计划（基于 Microsoft Agent Framework）

## 目标
使用 Microsoft Agent Framework 调用 qwen-long 模型分析已上传的文档（file_id: `file-fe-99019a488c19430fb32c7c67`）

## 技术方案

根据阿里云 DashScope 文档和 Microsoft Agent Framework：

1. **文件 ID 传入方式**：将 `file_id` 放入 System Message（instructions）中，格式为 `fileid://{file_id}`
2. **使用现有架构**：复用已配置的 OpenAIClient 和 Agent Framework
3. **调用方式**：通过 `AsAIAgent` 创建代理，使用 `RunAsync` 进行对话

## 实现步骤

### 步骤 1：修改 AIAgent 的 instructions
将文件 ID 加入 instructions 参数中：
```csharp
var instructions = $"fileid://{fileId}\n\n你是一个文档分析助手，请根据用户的问题分析文档内容。";
```

### 步骤 2：创建 AnalyzeDocumentAsync 方法
使用现有的 OpenAIClient 配置，创建专门用于文档分析的 Agent

### 步骤 3：调用 Agent 进行文档分析
使用 `agent.RunAsync(question)` 发送问题并获取分析结果

## 代码实现

```csharp
private static async Task<string> AnalyzeDocumentAsync(
    string apiKey, 
    string fileId, 
    string question)
{
    var baseUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1";

    var clientOptions = new OpenAIClientOptions()
    {
        Endpoint = new Uri(baseUrl)
    };

    var instructions = $"fileid://{fileId}";

    AIAgent agent = new OpenAIClient(
        new ApiKeyCredential(apiKey),
        clientOptions)
        .GetChatClient("qwen-long")
        .AsIChatClient()
        .AsAIAgent(instructions: instructions, name: "DocAnalyzer");

    var result = await agent.RunAsync(question);
    return result;
}
```

## 受影响的文件
- `Program.cs` - 添加 `AnalyzeDocumentAsync` 方法

## 验证方式
- 构建项目确保无编译错误
- 运行程序验证文档分析功能
