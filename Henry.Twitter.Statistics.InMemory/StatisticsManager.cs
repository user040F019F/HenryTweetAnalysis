using Henry.Twitter.Analyzation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Henry.Twitter.Statistics.InMemory
{
    class StatisticsManager
    {
        public int Total { get; private set; } = 0;

        internal int ContainingUrl = 0;
        internal int ContainingPhotoUrl = 0;
        internal int ContainingEmojis = 0;

        internal LeadingTracker<string> LeadingEmojiTracker = new LeadingTracker<string>();
        internal LeadingTracker<string> LeadingDomainTracker = new LeadingTracker<string>();
        internal LeadingTracker<string> LeadingHashtags = new LeadingTracker<string>();

        internal AverageTracker HourAverageTracker = new AverageTracker(3600, 24);
        internal AverageTracker MinuteAverageTracker = new AverageTracker(60, 60);
        internal AverageTracker SecondAverageTracker = new AverageTracker(1, 3600);

        internal void Handle(TweetAnalysis analysis)
        {
            Total++;
            if (analysis.HasPhotoUrl) ContainingPhotoUrl++;
            if (analysis.HasUrl) ContainingUrl++;
            bool hasEmojis = false;
            foreach(var emoji in analysis.Emojis)
            {
                LeadingEmojiTracker.Increment(emoji.Key, emoji.Value);
                hasEmojis = hasEmojis || emoji.Value > 0;
            }
            if (hasEmojis) ContainingEmojis++;
            foreach (var domain in analysis.Domains)
            {
                LeadingDomainTracker.Increment(domain, 1);
            }
            foreach (var tag in analysis.Tags)
            {
                LeadingHashtags.Increment(tag, 1);
            }
            HourAverageTracker.HandleAnalysis(analysis);
            MinuteAverageTracker.HandleAnalysis(analysis);
            SecondAverageTracker.HandleAnalysis(analysis);
        }
    }
}
