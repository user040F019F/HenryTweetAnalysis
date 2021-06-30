using Henry.Twitter.Shepherd.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd.Distribution
{
    class TweetChannelDispatcher<TCollected> : ITweetHandler<TCollected>
    {
        readonly ChannelProvider<TCollected> DistributionProvider;

        public TweetChannelDispatcher(
            ChannelProvider<TCollected> channelProvider)
        {
            DistributionProvider = channelProvider ?? throw new ArgumentNullException(nameof(channelProvider));
        }

        /// <inheritdoc/>
        /// <remarks>
        /// It would probably be better to send this to a proper event-sourcing system and broker
        /// to allow for distributed processing.. Just using multiple threads for now though (<see cref="ConcurrentTweetAnalysisService.ExecuteAsync(CancellationToken)"/>)
        /// </remarks>
        public async Task HandleAsync(TCollected tweet, CancellationToken cancellationToken)
        {
            await DistributionProvider.Distributor.Writer
                .WriteAsync(tweet, cancellationToken);
        }
    }
}
