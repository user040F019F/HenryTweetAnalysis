using Henry.Twitter.Analyzation.Abstractions;
using Henry.Twitter.Analyzation.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd.Distribution
{
    class DistributedTweetHandler<TCollected> : IDistributionHandler<TCollected>
    {
        readonly ITweetAnalyzer<TCollected> Analyzer;
        readonly ChannelProvider<TweetAnalysis> Distributor;

        public DistributedTweetHandler(
            ITweetAnalyzer<TCollected> analyzer,
            ChannelProvider<TweetAnalysis> distributor)
        {
            Analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
            Distributor = distributor ?? throw new ArgumentNullException(nameof(distributor));
        }

        public async Task ProcessAsync(TCollected collected, CancellationToken cancellationToken)
        {
            var analysis = await Analyzer.AnalyzeAsync(collected, cancellationToken);
            await Distributor.Distributor.Writer.WriteAsync(analysis, cancellationToken);
        }
    }
}
