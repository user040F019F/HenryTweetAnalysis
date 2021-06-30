using Henry.Twitter.Analyzation.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Analyzation.Abstractions
{
    public interface ITweetAnalysisHandler
    {
        Task HandleAsync(TweetAnalysis analysis, CancellationToken cancellationToken);
    }
}
