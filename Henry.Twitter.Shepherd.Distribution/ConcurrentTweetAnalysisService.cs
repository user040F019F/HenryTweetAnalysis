using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd.Distribution
{
    class ConcurrentTweetAnalysisService<TCollected> : BackgroundService
    {
        readonly ChannelDistributionProcessingStrategy<TCollected> Processor;

        public ConcurrentTweetAnalysisService(
            ChannelDistributionProcessingStrategy<TCollected> processor)
        {
            Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.WhenAll(
                Enumerable.Range(0, 50)
                    .Select(x => Processor.ProcessAsync(stoppingToken)));
        }
    }
}
