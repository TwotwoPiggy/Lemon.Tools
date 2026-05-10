using System;
using System.Net;
using Google.GenAI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Twotwo.Agent.Configuration;
using Twotwo.Agent.Interfaces;

namespace Twotwo.Agent.Services
{
    /// <summary>
    /// Gemini Provider implementation of IAgentFactory.
    /// Creates AIAgent instances backed by Google GenAI's IChatClient.
    /// 
    /// To switch to a different provider (OpenAI, Azure, Anthropic, etc.),
    /// create a new IAgentFactory implementation and register it in DI.
    /// </summary>
    public class GeminiAgentFactory : IAgentFactory
    {
        private readonly AIConfig _config;

        public GeminiAgentFactory(IOptions<AIConfig> config)
        {
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));

            if (string.IsNullOrEmpty(_config.ApiKey))
            {
                throw new ArgumentException("ApiKey is required in AIConfig.", nameof(config));
            }

            // Configure global proxy if needed
            if (_config.Proxy?.Enabled == true && !string.IsNullOrEmpty(_config.Proxy.Address))
            {
                WebRequest.DefaultWebProxy = new WebProxy(_config.Proxy.Address) { BypassProxyOnLocal = true };
            }
        }

        /// <inheritdoc />
        public AIAgent CreateAgent(string? instructions = null, string? name = null)
        {
            var modelName = _config.ModelName ?? "gemini-2.5-flash";
            var client = new Client(vertexAI: false, apiKey: _config.ApiKey);
            var chatClient = client.AsIChatClient(modelName);

            return new ChatClientAgent(
                chatClient,
                name: name ?? "TwotwoAgent",
                instructions: instructions ?? "You are a helpful assistant.");
        }
    }
}
