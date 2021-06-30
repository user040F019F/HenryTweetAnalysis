using Henry.Twitter.Shepherd.Invi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Microsoft.Extensions.Hosting;
using Henry.Twitter.Emoji.Configuration;
using System.Configuration;
using Henry.Twitter.Shepherd.Invi.Configuration;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Henry.Tester
{
    class Program
    {
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);
        
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            using var host = CreateHostBuilder(args)
                .ConfigureHostConfiguration(configuration => 
                    configuration.AddUserSecrets(Assembly.GetExecutingAssembly()))
                .ConfigureServices((context, services) =>
                {
                    var Configuration = context.Configuration;
                    services
                        .Configure<EmojiSettings>(Configuration.GetSection("emojis"), options => { options.BindNonPublicProperties = true; })
                        .Configure<TwitterCredentialSettings>(Configuration.GetSection("client"), options => { options.BindNonPublicProperties = true; })
                        .AddDefaultEmojiScanner()
                        .AddInviV2Support()
                        // Collect the tweets
                        .AddTransient<IInviV2TweetStreamProvider, InviV2SampleStreamProvider>()
                        .AddBasicTweetCollectionService<TweetV2>()
                        // Distribute the tweets to multiple threads for processing
                        .AddChannelDistributedTweetAnalyzer<TweetV2>()
                        // Store statistics in memory
                        .AddInMemoryTweetStatistics()
                        .AddHostedService<PollingService>();
                }).Build();
            await host.RunAsync();
        }
    }
}
