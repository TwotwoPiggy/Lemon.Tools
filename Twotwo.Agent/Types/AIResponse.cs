using Microsoft.Agents.AI;

namespace Twotwo.Agent.Types
{
    /// <summary>
    /// AI 响应基类，包含通用的文本回复。
    /// 框架重构后主要使用 AgentResponse，此类保留用于简化包装场景。
    /// </summary>
    public class AIResponse
    {
        public string OriginalResponse { get; set; }

        public AIResponse() { OriginalResponse = string.Empty; }

        public AIResponse(string text)
        {
            OriginalResponse = text;
        }

        /// <summary>
        /// 从框架的 AgentResponse 创建 AIResponse。
        /// </summary>
        public static AIResponse FromAgentResponse(AgentResponse response)
        {
            return new AIResponse(response?.ToString() ?? string.Empty);
        }
    }
}
