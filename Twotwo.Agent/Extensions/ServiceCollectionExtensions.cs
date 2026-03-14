using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twotwo.Agent.Configuration;
using Twotwo.Agent.Interfaces;
using Twotwo.Agent.Services;

namespace Twotwo.Agent.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGeminiAgent(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionName = "AIConfig")
        {
            services.Configure<AIConfig>(configuration.GetSection(configSectionName));
            services.AddSingleton<IGeminiAgentService, GeminiAgentService>();
            
            return services;
        }

        public static IServiceCollection AddGeminiAgent(
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
            
            services.AddSingleton<IGeminiAgentService, GeminiAgentService>();
            
            return services;
        }

        public static IServiceCollection AddGeminiAgent(
            this IServiceCollection services,
            Action<AIConfig> configure)
        {
            services.Configure(configure);
            services.AddSingleton<IGeminiAgentService, GeminiAgentService>();
            
            return services;
        }
    }
}
