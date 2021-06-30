using Henry.Twitter.Shepherd;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionEtensions
    {
        public static IServiceCollection AddBasicTweetCollectionService<TCollectable>(
            this IServiceCollection services)
        {
            return services
                .AddHostedService<TweetCollectionService<TCollectable>>();
        }
    }
}
