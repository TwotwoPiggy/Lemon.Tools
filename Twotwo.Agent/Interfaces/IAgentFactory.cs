using Microsoft.Agents.AI;

namespace Twotwo.Agent.Interfaces
{
    /// <summary>
    /// Provider-agnostic factory for creating AIAgent instances.
    /// Swap the implementation to switch providers (Gemini, OpenAI, Azure, etc.)
    /// without changing any calling code.
    /// </summary>
    public interface IAgentFactory
    {
        /// <summary>
        /// Creates an AIAgent using the configured provider.
        /// </summary>
        /// <param name="instructions">System instructions for the agent.</param>
        /// <param name="name">Optional agent name.</param>
        /// <returns>A ready-to-use AIAgent instance.</returns>
        AIAgent CreateAgent(string? instructions = null, string? name = null);
    }
}
