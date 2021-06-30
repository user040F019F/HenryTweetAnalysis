using Henry.Twitter.Analyzation.Abstractions;
using Henry.Twitter.Analyzation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Models.V2;

namespace Henry.Twitter.Shepherd.Invi
{
    class InviTweetAnalyer : ITweetAnalyzer<TweetV2>
    {
        readonly IEmojiScanner EmojiScanner;

        public InviTweetAnalyer(IEmojiScanner emojiScanner)
        {
            EmojiScanner = emojiScanner ?? throw new ArgumentNullException(nameof(emojiScanner));
        }

        public Task<TweetAnalysis> AnalyzeAsync(TweetV2 tweet, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                new TweetAnalysis(
                    tweet.Id,
                    tweet.CreatedAt,
                    new HashSet<string>(tweet.Entities.Urls?.Select(x => new Uri(x.ExpandedUrl).Host) ?? Enumerable.Empty<string>(), StringComparer.CurrentCultureIgnoreCase),
                    new HashSet<string>(tweet.Entities.Hashtags?.Select(x => x.Tag) ?? Enumerable.Empty<string>(), StringComparer.CurrentCultureIgnoreCase),
                    EmojiScanner.GetMatches(tweet.Text).Cast<Match>().GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.Count())));
        }
    }
}
