using Henry.Twitter.Analyzation.Abstractions;
using Henry.Twitter.Statistics.Abstractions;
using Henry.Twitter.Statistics.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryTweetStatistics(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<StatisticsManager>()
                .AddTransient<ITweetAnalysisHandler, AnalysisHandler>()
                .AddTransient<ITweetLeadingDomainProvider, StatisticsProvider>()
                .AddTransient<ITweetLeadingEmojiProvider, StatisticsProvider>()
                .AddTransient<ITweetLeadingHashtagProvider, StatisticsProvider>()
                .AddTransient<ITweetPeriodAverageProvider, StatisticsProvider>()
                .AddTransient<ITweetTotalProvider, StatisticsProvider>()
                .AddTransient<ITweetWithEmojisPercentageProvider, StatisticsProvider>()
                .AddTransient<ITweetWithPhotoUrlPercentageProvider, StatisticsProvider>()
                .AddTransient<ITweetWithUrlPercentageProvider, StatisticsProvider>();

        }
    }
}
