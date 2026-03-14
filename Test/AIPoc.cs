using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Twotwo.Agent.Configuration;
using Twotwo.Agent.Constants;
using Twotwo.Agent.Interfaces;
using Twotwo.Agent.Services;
using Twotwo.Agent.Extensions;

namespace Test
{
    public class AIPoc
    {
        public static async Task RunTest()
        {
            var config = new AIConfig
            {
                ApiKey = "",
                //ModelName = "gemini-3.1-flash-lite-preview",//model not found
                ModelName = "gemini-2.5-flash",
                Proxy = new ProxyConfig { Enabled = true, Address = "http://127.0.0.1:10808" }
            };

            var services = new ServiceCollection();
            services.AddGeminiAgent(config);
            var serviceProvider = services.BuildServiceProvider();
            
            var agentService = serviceProvider.GetRequiredService<IGeminiAgentService>();
            await agentService.ValidateModelAsync();

            PromptLoader.Load("Prompts.json");

            byte[] imageBytes1 = File.ReadAllBytes(@"C:\Users\Lemony\Desktop\PDD\testfood.jpg");
            byte[] imageBytes2 = File.ReadAllBytes(@"C:\Users\Lemony\Desktop\PDD\catfood.jpg");
            var request = new AIRequest(
                Text: PromptLoader.Get(PromptConstants.OcrAssistantPrompt),
                Files: new List<byte[]> { imageBytes1, imageBytes2 },
                MimeType: "image/jpeg"
            );

            try
            {
                var response = await agentService.GenerateContentAsync(request);
                Console.WriteLine(response.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
