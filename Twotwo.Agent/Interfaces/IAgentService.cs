using System.Collections.Generic;
using System.Threading.Tasks;
using Twotwo.Agent.Types;

namespace Twotwo.Agent.Interfaces
{
    public record AIRequest(string Text, List<byte[]>? Files = null, string? MimeType = null, string? ModelName = null);

    public interface IAgentService<T> where T : AIResponse
    {
        Task<T> GenerateContentAsync(AIRequest request);
    }
}
