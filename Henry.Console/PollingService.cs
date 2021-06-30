using Henry.Twitter.Statistics.Abstractions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Tester
{
    class PollingService : BackgroundService
    {
        readonly ITweetLeadingDomainProvider LeadingDomainProvider;
        readonly ITweetLeadingEmojiProvider LeadingEmojiProvider;
        readonly ITweetLeadingHashtagProvider LeadingHashtagProvider;
        readonly ITweetPeriodAverageProvider TweetPeriodAverageProvider;
        readonly ITweetTotalProvider TweetTotalProvider;
        readonly ITweetWithEmojisPercentageProvider TweetWithEmojisPercentageProvider;
        readonly ITweetWithPhotoUrlPercentageProvider TweetWithPhotoUrlPercentageProvider;
        readonly ITweetWithUrlPercentageProvider TweetWithUrlPercentageProvider;

        public PollingService(
            ITweetLeadingDomainProvider leadingDomainProvider,
            ITweetLeadingEmojiProvider leadingEmojiProvider,
            ITweetLeadingHashtagProvider leadingHashtagProvider,
            ITweetPeriodAverageProvider tweetPeriodAverageProvider,
            ITweetTotalProvider tweetTotalProvider,
            ITweetWithEmojisPercentageProvider tweetWithEmojisPercentageProvider,
            ITweetWithPhotoUrlPercentageProvider tweetWithPhotoUrlPercentageProvider,
            ITweetWithUrlPercentageProvider tweetWithUrlPercentageProvider)
        {
            LeadingDomainProvider = leadingDomainProvider ?? throw new ArgumentNullException(nameof(leadingDomainProvider));
            LeadingEmojiProvider = leadingEmojiProvider ?? throw new ArgumentNullException(nameof(leadingEmojiProvider));
            LeadingHashtagProvider = leadingHashtagProvider ?? throw new ArgumentNullException(nameof(leadingHashtagProvider));
            TweetPeriodAverageProvider = tweetPeriodAverageProvider ?? throw new ArgumentNullException(nameof(tweetPeriodAverageProvider));
            TweetTotalProvider = tweetTotalProvider ?? throw new ArgumentNullException(nameof(tweetTotalProvider));
            TweetWithEmojisPercentageProvider = tweetWithEmojisPercentageProvider ?? throw new ArgumentNullException(nameof(tweetWithEmojisPercentageProvider));
            TweetWithPhotoUrlPercentageProvider = tweetWithPhotoUrlPercentageProvider ?? throw new ArgumentNullException(nameof(tweetWithPhotoUrlPercentageProvider));
            TweetWithUrlPercentageProvider = tweetWithUrlPercentageProvider ?? throw new ArgumentNullException(nameof(tweetWithUrlPercentageProvider));
        }

        static string EscapeUnicode(string value)
        {
            return string.Concat(value.Select(x => x < 128 ? x.ToString() : string.Format(@"\u{0:x4}", (int)x)));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Polling statistics...");
                try
                {
                    var total = await TweetTotalProvider.GetTotalAsync(stoppingToken);
                    if (total > 0)
                    {
                        var statistics = new
                        {
                            Total = await TweetTotalProvider.GetTotalAsync(stoppingToken),
                            Averages = new
                            {
                                Hour = await TweetPeriodAverageProvider.GetAverageAsync('h', stoppingToken),
                                Minute = await TweetPeriodAverageProvider.GetAverageAsync('m', stoppingToken),
                                Second = await TweetPeriodAverageProvider.GetAverageAsync('s', stoppingToken)
                            },
                            LeadingDomains = await LeadingDomainProvider.GetLeadingAsync(5, stoppingToken),
                            LeadingEmojis = await LeadingEmojiProvider.GetLeadingAsync(5, stoppingToken),
                            LeadingTags = await LeadingHashtagProvider.GetLeadingAsyc(5, stoppingToken),
                            WithEmojiPercentage = await TweetWithEmojisPercentageProvider.GetPercentageAsync(stoppingToken),
                            WithUrlPercentage = await TweetWithUrlPercentageProvider.GetPercentageAsync(stoppingToken),
                            WithPhotoUrlPercentage = await TweetWithPhotoUrlPercentageProvider.GetPercentageAsync(stoppingToken)
                        };

                        Console.WriteLine($"Total tweets received: {statistics.Total}");
                        Console.Write($"Average tweets: ");
                        Console.WriteLine(string.Join(',',
                            $"{statistics.Averages.Hour}/h",
                            $"{statistics.Averages.Minute}/m",
                            $"{statistics.Averages.Second}/s"));
                        Console.WriteLine($"Top 5 emojis: {string.Join(',', statistics.LeadingEmojis.Select(EscapeUnicode))}");
                        Console.WriteLine($"Percentage of tweets with emojis: {statistics.WithEmojiPercentage}%");
                        Console.WriteLine($"Top 5 hashtags: {string.Join(',', statistics.LeadingTags.Select(EscapeUnicode))}");
                        Console.WriteLine($"Percentage of tweets with url: {statistics.WithUrlPercentage}%");
                        Console.WriteLine($"Percentage of tweets with photo url: {statistics.WithPhotoUrlPercentage}%");
                        Console.WriteLine($"Top 5 domains: {string.Join(',', statistics.LeadingDomains)}");
                    }
                    else
                    {
                        Console.WriteLine("No tweets received yet...");
                    }
                } 
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while polling statistics.");
                }
                Console.WriteLine();
                try
                {
                    await Task.Delay(5000);
                }
                catch (TaskCanceledException)
                {
                    // Quiet cleanup
                }
            }
        }
    }
}
