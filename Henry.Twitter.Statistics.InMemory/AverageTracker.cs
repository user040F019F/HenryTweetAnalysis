using Henry.Twitter.Analyzation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Henry.Twitter.Statistics.InMemory
{
    class AverageTracker
    {
        readonly int Resolution;
        readonly int Limit;
        readonly long?[] Stamps;
        readonly int[] Samples;
        object mutex = new object();

        public AverageTracker(
            int resolution,
            int limit)
        {
            Resolution = resolution;
            Limit = limit;
            Stamps = new long?[limit];
            Samples = new int[limit];
        }

        long GetStamp(DateTimeOffset timestamp)
        {
            return timestamp.ToUnixTimeSeconds() / Resolution;
        }

        public void HandleAnalysis(TweetAnalysis analysis)
        {
            lock (mutex)
            {
                var threshold = GetStamp(DateTimeOffset.Now) - Limit;
                var stamp = GetStamp(analysis.CreatedAt);
                // Ignore old, irrelevant stamps
                if (stamp < threshold) return;
                var index = (int)(stamp % Limit);
                if (stamp != Stamps[index]) Samples[index] = 0;
                Stamps[index] = stamp;
                Samples[index]++;
            }
        }

        public decimal Calculate()
        {
            lock (mutex)
            {
                var current = GetStamp(DateTimeOffset.Now);
                var (count, total) = Samples
                    .Where((x, i) => Stamps[i].HasValue && Stamps[i] != current)
                    .Aggregate(
                        (count: 0, total: 0),
                        (aggregate, sample) =>
                        (
                            count: aggregate.count + 1,
                            total: aggregate.total + sample
                        ));
                return count > 0 ? ((decimal)total) / count : count;
            }
        }
    }
}
