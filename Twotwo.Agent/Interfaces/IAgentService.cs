using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Agents.AI;

namespace Twotwo.Agent.Interfaces
{
    /// <summary>
    /// Data contract for AI requests. Preserved from v1 for backward compatibility.
    /// </summary>
    public record AIRequest(
        string Text, 
        List<byte[]>? Files = null, 
        string? MimeType = null, 
        string? ModelName = null);

    /// <summary>
    /// Provider-agnostic agent service interface.
    /// Wraps AIAgent with a simplified API surface for common use cases.
    /// </summary>
    public interface IAgentService
    {
        /// <summary>
        /// Run a simple text prompt through the agent.
        /// </summary>
        Task<AgentResponse> RunAsync(string prompt, CancellationToken cancellationToken = default);

        /// <summary>
        /// Run a structured AIRequest (supports multimodal files).
        /// </summary>
        Task<AgentResponse> RunAsync(AIRequest request, CancellationToken cancellationToken = default);
    }
}
