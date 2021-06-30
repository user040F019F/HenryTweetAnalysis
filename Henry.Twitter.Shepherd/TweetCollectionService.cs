using Henry.Twitter.Shepherd.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd
{
    public class TweetCollectionService<TCollectable> : BackgroundService
    {
        readonly ILogger<TweetCollectionService<TCollectable>> Logger;
        readonly ITweetCollectionStreamStrategy<TCollectable> StreamStrategy;
        readonly ITweetHandler<TCollectable> Handler;

        public TweetCollectionService(
            ILogger<TweetCollectionService<TCollectable>> logger,
            ITweetCollectionStreamStrategy<TCollectable> collectionStrategy,
            ITweetHandler<TCollectable> handler)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            StreamStrategy = collectionStrategy ?? throw new ArgumentNullException(nameof(collectionStrategy));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach(var collected in StreamStrategy.StreamAsync(stoppingToken))
            {
                try
                {
                    await Handler.HandleAsync(collected, stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occured while handling a collected tweet.");
                }
            }
        }
    }
}
