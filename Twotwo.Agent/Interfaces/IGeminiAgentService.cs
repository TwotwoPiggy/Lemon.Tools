using System.Threading;
using System.Threading.Tasks;
using Twotwo.Agent.Types;

namespace Twotwo.Agent.Interfaces
{
    public interface IGeminiAgentService : IAgentService<GeminiResponse>
    {
        Task<bool> ValidateModelAsync(CancellationToken cancellationToken = default);
    }
}
