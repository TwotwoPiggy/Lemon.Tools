using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twotwo.Agent.Configuration;
using Twotwo.Agent.Interfaces;
using Twotwo.Agent.Services;

namespace Twotwo.Agent.Extensions
{
    /// <summary>
    /// DI extension methods for registering AI Agent services.
    /// Provider-agnostic: default uses Gemini, override IAgentFactory to switch.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register AI Agent services using IConfiguration binding.
        /// Default provider: Gemini (via GeminiAgentFactory).
        /// </summary>
        public static IServiceCollection AddAgent(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionName = "AIConfig")
        {
            services.Configure<AIConfig>(configuration.GetSection(configSectionName));
            services.AddSingleton<IAgentFactory, GeminiAgentFactory>();
            services.AddSingleton<IAgentService, AgentService>();
            return services;
        }

        /// <summary>
        /// Register AI Agent services using a pre-built AIConfig object.
        /// </summary>
        public static IServiceCollection AddAgent(
            this IServiceCollection services,
            AIConfig config)
        {
            services.Configure<AIConfig>(options =>
            {
                options.ApiKey = config.ApiKey;
                options.ModelName = config.ModelName;
                options.RPM = config.RPM;
                options.TPM = config.TPM;
                options.RPD = config.RPD;
                options.Proxy = config.Proxy;
            });
            services.AddSingleton<IAgentFactory, GeminiAgentFactory>();
            services.AddSingleton<IAgentService, AgentService>();
            return services;
        }

        /// <summary>
        /// Register AI Agent services using an Action configurator.
        /// </summary>
        public static IServiceCollection AddAgent(
            this IServiceCollection services,
            Action<AIConfig> configure)
        {
            services.Configure(configure);
            services.AddSingleton<IAgentFactory, GeminiAgentFactory>();
            services.AddSingleton<IAgentService, AgentService>();
            return services;
        }

        /// <summary>
        /// Register AI Agent services with a custom IAgentFactory implementation.
        /// Use this to plug in non-Gemini providers (OpenAI, Azure, Anthropic, etc.)
        /// </summary>
        /// <typeparam name="TFactory">The IAgentFactory implementation type.</typeparam>
        public static IServiceCollection AddAgent<TFactory>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionName = "AIConfig")
            where TFactory : class, IAgentFactory
        {
            services.Configure<AIConfig>(configuration.GetSection(configSectionName));
            services.AddSingleton<IAgentFactory, TFactory>();
            services.AddSingleton<IAgentService, AgentService>();
            return services;
        }
    }
}
