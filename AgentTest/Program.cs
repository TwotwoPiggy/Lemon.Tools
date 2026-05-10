using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

namespace AgentTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var apiKey = "sk-07ee8dd4d8d24c639e3020e8c50fa754";
            var model = "qwen-long";
            var baseUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1";
            var fileId = "file-fe-39fffe378cd14a82a699001b";
            //var fileId = "file-fe-99019a488c19430fb32c7c67";
            //await UploadFileAsync(apiKey, @"C:\Users\Lemony\Desktop\sharing\Harness_Engineering_Architecture.pptx");
            await AnalyzeDocumentAsync(apiKey, fileId, "What is the content of this file?");
            //var clientOptions = new OpenAIClientOptions()
            //{
            //    Endpoint = new Uri(baseUrl)
            //};
            //AIAgent agent = new OpenAIClient(
            //     new ApiKeyCredential(apiKey),
            //     clientOptions)
            //     .GetChatClient(model)
            //     .AsIChatClient()
            //     .AsAIAgent(instructions: "你是一个文档填充助手", name: "DocFiller");

            //// Invoke the agent and output the text result.
            //Console.WriteLine(await agent.RunAsync("Tell me a joke about a pirate."));
        }

        private static async Task UploadFileAsync(string apiKey, string filePath)
        {
            var baseUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1";

            using var httpClient = new HttpClient();
            using var formContent = new MultipartFormDataContent();

            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var fileContent = new ByteArrayContent(fileBytes);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            var fileName = Path.GetFileName(filePath);
            formContent.Add(fileContent, "file", fileName);
            formContent.Add(new StringContent("file-extract"), "purpose");

            var response = await httpClient.PostAsync($"{baseUrl}/files", formContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine($"Response: {responseBody}");
        }

        private static async Task<string> AnalyzeDocumentAsync(
            string apiKey,
            string fileId,
            string question)
        {
            var baseUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1";

            var clientOptions = new OpenAIClientOptions()
            {
                Endpoint = new Uri(baseUrl)
            };

            var instructions = $"fileid://{fileId}";

            AIAgent agent = new OpenAIClient(
                new ApiKeyCredential(apiKey),
                clientOptions)
                .GetChatClient("qwen-long")
                .AsIChatClient()
                .AsAIAgent(instructions: instructions, name: "DocAnalyzer");

            var systemPrompt = """
                你是一位专业的文档分析专家，擅长从各类文档中提取关键信息、总结核心内容，并提供深入的分析见解。

                请基于文档内容回答用户的问题，遵循以下原则：
                1. 仅基于文档内容回答，不要编造不存在的信息
                2. 如果文档中没有相关信息，请明确告知
                3. 回答要准确、客观
                4. 使用清晰的段落结构组织回答
                5. 必要时引用文档原文作为依据

                用户问题：
                """;

            var result = await agent.RunAsync(systemPrompt + question);
            return result.Text;
        }
    }
}
