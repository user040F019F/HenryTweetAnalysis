using Henry.Twitter.Analyzation.Abstractions;
using Henry.Twitter.Shepherd.Abstractions;
using Henry.Twitter.Shepherd.Invi;
using Henry.Twitter.Shepherd.Invi.Configuration;
using Microsoft.Extensions.Options;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add TweetV2 collection and analysis support
        /// </summary>
        public static IServiceCollection AddInviV2Support(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<ITwitterClient>(provider =>
                {
                    var settings = provider
                        .GetRequiredService<IOptions<TwitterCredentialSettings>>()
                        .Value;
                    var client = new TwitterClient(
                        new ConsumerOnlyCredentials(
                            settings.ConsumerKey,
                            settings.ConsumerSecret));
                    client.Auth.InitializeClientBearerTokenAsync();
                    return client;
                })
                .AddTransient<ITweetCollectionStreamStrategy<TweetV2>, InviStreamCollectionStrategy>()
                .AddTransient<ITweetAnalyzer<TweetV2>, InviTweetAnalyer>();
        }
    }
}
