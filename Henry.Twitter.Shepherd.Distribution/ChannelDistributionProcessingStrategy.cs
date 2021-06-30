using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd.Distribution
{
    class ChannelDistributionProcessingStrategy<TDistributed>
    {
        readonly ChannelProvider<TDistributed> DistributorProvider;
        readonly ILogger<ChannelDistributionProcessingStrategy<TDistributed>> Logger;
        readonly IDistributionHandler<TDistributed> Strategy;

        public ChannelDistributionProcessingStrategy(
            ChannelProvider<TDistributed> distributorProvider,
            ILogger<ChannelDistributionProcessingStrategy<TDistributed>> logger,
            IDistributionHandler<TDistributed> strategy)
        {
            DistributorProvider = distributorProvider ?? throw new ArgumentNullException(nameof(distributorProvider));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public async Task ProcessAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var value = await DistributorProvider.Distributor.Reader.ReadAsync(stoppingToken);
                    if (stoppingToken.IsCancellationRequested) break;
                    await Strategy.ProcessAsync(value, stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "There was an error processing a chennel message.");
                }
            }
        }
    }
}
