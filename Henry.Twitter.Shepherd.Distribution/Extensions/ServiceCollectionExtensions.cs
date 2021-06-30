using Henry.Twitter.Analyzation.Models;
using Henry.Twitter.Shepherd.Abstractions;
using Henry.Twitter.Shepherd.Distribution;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChannelDistributedTweetAnalyzer<TCollected>(
            this IServiceCollection services)
        {
            return services
                .AddTransient<IChannelFactory<TCollected>, CollectedTweetChannelFactory<TCollected>>()
                .AddTransient<IChannelFactory<TweetAnalysis>, TweetAnalysisChannelFactory>()
                .AddSingleton<ChannelProvider<TCollected>>()
                .AddSingleton<ChannelProvider<TweetAnalysis>>()
                .AddTransient<ITweetHandler<TCollected>, TweetChannelDispatcher<TCollected>>()
                .AddTransient<IDistributionHandler<TCollected>, DistributedTweetHandler<TCollected>>()
                .AddTransient<IDistributionHandler<TweetAnalysis>, DistributedAnalysisHandler>()
                .AddTransient(typeof(ChannelDistributionProcessingStrategy<>))
                .AddHostedService<ConcurrentTweetAnalysisService<TCollected>>()
                .AddHostedService<TweetAnalysisHandlingService>();
        }
    }
}
