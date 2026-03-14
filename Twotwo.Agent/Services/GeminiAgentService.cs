using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Twotwo.Agent.Configuration;
using Twotwo.Agent.Interfaces;
using Twotwo.Agent.Types;

namespace Twotwo.Agent.Services
{
    public class GeminiAgentService : IGeminiAgentService
    {
        private readonly Client _client;
        private readonly string _defaultModelName;
        private readonly AIConfig _config;
        private bool _modelValidated;

        public GeminiAgentService(IOptions<AIConfig> config)
        {
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
            
            if (string.IsNullOrEmpty(_config.ApiKey))
            {
                throw new ArgumentException("ApiKey is required in AIConfig.", nameof(config));
            }

            if (_config.Proxy?.Enabled == true && !string.IsNullOrEmpty(_config.Proxy.Address))
            {
                HttpClient.DefaultProxy = new WebProxy(_config.Proxy.Address) { BypassProxyOnLocal = true };
            }

            _client = new Client(apiKey: _config.ApiKey);
            _defaultModelName = _config.ModelName ?? "gemini-2.5-flash";
        }

        public async Task<bool> ValidateModelAsync(CancellationToken cancellationToken = default)
        {
            if (_modelValidated) return true;

            var modelNameToCheck = $"models/{_defaultModelName}";
            try
            {
                var models = await _client.Models.ListAsync(cancellationToken: cancellationToken);
                var modelExists = await models.FirstOrDefaultAsync(
                    model => model.Name == modelNameToCheck,
                    cancellationToken: cancellationToken);

                var isValid = modelExists != null && 
                              modelExists.SupportedActions.Contains("generateContent");

                if (!isValid)
                {
                    throw new ArgumentException(
                        $"Model '{_defaultModelName}' is not a valid, available model.",
                        nameof(_config.ModelName));
                }

                _modelValidated = true;
                return true;
            }
            catch (Exception ex) when (ex is not ArgumentException)
            {
                throw new InvalidOperationException(
                    "Failed to retrieve available models from Gemini client.", ex);
            }
        }

        public async Task<GeminiResponse> GenerateContentAsync(AIRequest request)
        {
            var contentParts = new List<Part> { new Part { Text = request.Text } };
            
            if (request.Files != null && request.MimeType != null)
            {
                foreach (var fileBytes in request.Files)
                {
                    contentParts.Insert(0, Part.FromBytes(fileBytes, request.MimeType));
                }
            }

            var content = new Content { Parts = contentParts };

            var response = await _client.Models.GenerateContentAsync(
                model: request.ModelName ?? _defaultModelName,
                contents: content
            );

            var text = JsonConvert.SerializeObject(response);

            return new GeminiResponse(text)
            {
                Candidates = response.Candidates,
                ModelVersion = response.ModelVersion,
                PromptFeedback = response.PromptFeedback,
                UsageMetadata = response.UsageMetadata,
                Text = response.Candidates?[0].Content?.Parts?[0].Text ?? "No content returned.",
                Parts = response.Candidates?[0].Content?.Parts
            };
        }
    }
}
