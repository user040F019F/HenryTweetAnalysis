using Henry.Twitter.Analyzation.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd.Distribution
{
    class TweetAnalysisHandlingService : BackgroundService
    {
        readonly ChannelDistributionProcessingStrategy<TweetAnalysis> Processor;

        public TweetAnalysisHandlingService(
            ChannelDistributionProcessingStrategy<TweetAnalysis> processor)
        {
            Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
            Processor.ProcessAsync(stoppingToken);
    }
}
