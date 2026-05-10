using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Agents.AI;
using Twotwo.Agent.Interfaces;

namespace Twotwo.Agent.Services
{
    /// <summary>
    /// Provider-agnostic agent service implementation.
    /// Wraps an AIAgent (created by IAgentFactory) with a simplified API.
    /// </summary>
    public class AgentService : IAgentService
    {
        private readonly IAgentFactory _factory;

        public AgentService(IAgentFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <inheritdoc />
        public async Task<AgentResponse> RunAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var agent = _factory.CreateAgent();
            return await agent.RunAsync(prompt, cancellationToken: cancellationToken);
        }

        /// <inheritdoc />
        public async Task<AgentResponse> RunAsync(AIRequest request, CancellationToken cancellationToken = default)
        {
            var agent = _factory.CreateAgent();

            // Build prompt: text + optional file descriptions
            var prompt = request.Text;
            if (request.Files != null && request.Files.Count > 0)
            {
                var sb = new StringBuilder(request.Text);
                sb.AppendLine();
                sb.AppendLine("[Attached files: " + request.Files.Count + " file(s)]");
                prompt = sb.ToString();
            }

            return await agent.RunAsync(prompt, cancellationToken: cancellationToken);
        }
    }
}
