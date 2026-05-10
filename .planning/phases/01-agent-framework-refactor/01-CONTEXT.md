# Phase: Twotwo.Agent 重构 - Context

**Gathered:** 2026-04-15
**Status:** Ready for planning

<domain>
## Phase Boundary

将 `Twotwo.Agent` 从手写的 AI 调用层重构为基于 **Microsoft Agent Framework** 的标准化代理架构。

核心原则：**扩展性和通用性优先** — 为未来可能更优秀的模型、Provider、框架预留无缝迁移、替换和扩展的最大空间。

不包含：UI 层变更、多代理 Workflow 编排（这是未来阶段的事）。
</domain>

<decisions>
## Implementation Decisions

### Provider 策略
- **D-01:** 以 `Microsoft.Extensions.AI.IChatClient` 为核心抽象协议 — 这是整个框架的扩展点，任何满足该接口的 Provider 都可以即插即用。
- **D-02:** 当前 Gemini Provider 使用 `Google.GenAI` 官方 SDK（`.AsIChatClient()` 扩展方法），**不使用** `Mscc.GenerativeAI.Microsoft` 社区包。
- **D-03:** 框架通过 `IChatClient` 已内置支持 Azure OpenAI / OpenAI / Anthropic / Ollama / GitHub Copilot / 自定义 Provider，切换 Provider 只需替换注入的 `IChatClient`，上层代码零改动。

### 接口策略（API 层设计）
- **D-04:** 移除现有的 `IAgentService<T>` 泛型接口设计，改为面向 `Microsoft.Agents.AI.AIAgent` 的抽象层。
- **D-05:** 对外暴露的公共接口基于 `AIAgent` 抽象：
  - `AIAgent` 是框架统一基类，所有 Provider 的代理都派生自它
  - 提供简洁的 ``IAgent` 接口（或直接使用框架的 `AIAgent`），调用方无需感知具体 Provider
- **D-06:** `AIRequest` record 保留（作为调用入口的数据契约），内部转换为框架的 `ChatMessage[]` 格式。
- **D-07:** 用 `IChatClient` + `ChatClientAgent` 作为标准 Provider 接入路径：
  ```
  IChatClient（任意 Provider SDK 提供）
      → ChatClientAgent（框架统一包装）
      → AIAgent（对外暴露的统一基类）
  ```

### 范围
- **D-08:** 本次重构 **不绑定** 任何特定 AI 厂商，架构上 Provider-agnostic。
- **D-09:** 当前实现 Gemini，但架构设计上同时预留 OpenAI / Azure / Anthropic / Ollama 等接入点的清晰扩展路径。
- **D-10:** 旧的 `GeminiAgentService` 和 `IGeminiAgentService` 接口迁移至基于 `ChatClientAgent` 的实现。

### 清理
- **D-11:** `AgentTest.cs` 是临时测试文件，重构完成后删除。
- **D-12:** `IsExternalInit.cs` polyfill 保留（netstandard2.1 兼容性所需）。

### Agent 的Discretion
- 具体的 DI 注册模式（`AddTransient` / `AddSingleton` / `AddScoped`）由 agent 决定，以 `IChatClient` 的生命周期为准。
- `AIRequest` 内部转换为 `ChatMessage[]` 的具体映射逻辑由 agent 实现。
</decisions>

<canonical_refs>
## Canonical References

**Downstream agents MUST read these before planning or implementing.**

### Microsoft Agent Framework
- `https://learn.microsoft.com/zh-cn/agent-framework/overview` — 框架总览，核心概念
- `https://learn.microsoft.com/zh-cn/agent-framework/agents/index` — Agent 类型：ChatClientAgent / AIAgent / 自定义 Agent
- `https://learn.microsoft.com/zh-cn/agent-framework/agents/providers/index` — Provider 概览：IChatClient 接入点，支持的所有 Provider

### GitHub 源码示例
- `https://github.com/microsoft/agent-framework/tree/main/dotnet/samples` — C# 示例代码（重点参考）

### 当前项目文件
- `d:\Computers\develop\.NetCore\0.Lemon\Twotwo.Agent\Twotwo.Agent.csproj` — 项目文件（netstandard2.1, LangVersion 10.0）
- `d:\Computers\develop\.NetCore\0.Lemon\Twotwo.Agent\Interfaces\IAgentService.cs` — 待重构的接口
- `d:\Computers\develop\.NetCore\0.Lemon\Twotwo.Agent\Interfaces\IGeminiAgentService.cs` — 待重构的 Gemini 接口
- `d:\Computers\develop\.NetCore\0.Lemon\Twotwo.Agent\Services\GeminiAgentService.cs` — 待重构的 Gemini 实现
- `d:\Computers\develop\.NetCore\0.Lemon\Twotwo.Agent\Services\AgentTest.cs` — 临时测试文件，重构后删除

### 外部依赖（NuGet）
- `Microsoft.Agents.AI.Foundry` — 当前已引入，Agent Framework 核心包
- `Google.GenAI` — Gemini 官方 SDK，提供 `.AsIChatClient()` 扩展
</canonical_refs>

<code_context>
## Existing Code Insights

### Reusable Assets
- `AIRequest` record — 保留作为对外数据契约，内部适配为 `ChatMessage[]`
- `AIConfig` / `ProxyConfig` — Configuration 配置类，保留
- `PromptLoader.cs` — 保留，用于加载 Prompts.json 中的系统提示词
- `IsExternalInit.cs` — 保留，netstandard2.1 兼容性 polyfill

### Established Patterns
- DI 通过 `IOptions<AIConfig>` 注入配置（继续沿用）
- `WebRequest.DefaultWebProxy` 作为代理设置入口（已在 netstandard2.1 下修复）
- 异步 `Task<T>` + `CancellationToken` 的 async 模式（继续沿用）

### Integration Points
- 外部调用方通过 DI 注入 Agent 实例，调用 `RunAsync(string)` 或 `GenerateContentAsync(AIRequest)`
- 未来多代理 Workflow 从 `AIAgent` 基类组合，接入方式与本次重构完全兼容
</code_context>

<specifics>
## Specific Ideas

- 用户明确强调：**扩展性和通用性优先于功能完整性**。架构上的决定要为未来的 Provider 替换留白，而不是过度优化当前的 Gemini 实现。
- Agent Framework 的 `IChatClient` 抽象是实现这个目标的关键工具 — "任何满足 IChatClient 接口的 Provider 都可以即插即用"就是用户期望的扩展模型。
- 最终形态应该是：调用方只接触 `AIAgent` 抽象，对底层 Gemini / OpenAI / Anthropic 无感知。
</specifics>

<deferred>
## Deferred Ideas

- **多代理 Workflow 编排**（Sequential / Concurrent / Graph）— 框架支持，但属于高阶功能，放到后续阶段规划。
- **Memory / 持久化聊天历史**（基于会话）— 放到后续阶段。
- **工具调用（Function Calling / MCP）**— 框架原生支持，放到后续阶段。
- **流式响应（Streaming）**— 框架支持，当前阶段不强制要求，后续可以通过 `AgentResponseUpdate` 扩展。
- **A2A 协议支持** — 远程 Agent 协作，属于高阶集成，放到后续阶段。
</deferred>
