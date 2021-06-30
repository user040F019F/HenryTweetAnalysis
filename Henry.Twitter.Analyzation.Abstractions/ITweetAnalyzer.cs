using Henry.Twitter.Analyzation.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Analyzation.Abstractions
{
    public interface ITweetAnalyzer<in TCollected>
    {
        Task<TweetAnalysis> AnalyzeAsync(TCollected tweet, CancellationToken cancellationToken = default);
    }
}
