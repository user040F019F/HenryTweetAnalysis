using Henry.Twitter.Statistics.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Statistics.InMemory
{
    class StatisticsProvider :
        ITweetLeadingDomainProvider,
        ITweetLeadingEmojiProvider,
        ITweetLeadingHashtagProvider,
        ITweetPeriodAverageProvider,
        ITweetTotalProvider,
        ITweetWithEmojisPercentageProvider,
        ITweetWithPhotoUrlPercentageProvider,
        ITweetWithUrlPercentageProvider
    {
        readonly StatisticsManager Manager;

        public StatisticsProvider(
            StatisticsManager manager)
        {
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        Task<decimal> ITweetPeriodAverageProvider.GetAverageAsync(char period, CancellationToken cancellationToken)
        {
            decimal? result = null;
            switch (period)
            {
                case 'h': result = Manager.HourAverageTracker.Calculate(); break;
                case 'm': result = Manager.MinuteAverageTracker.Calculate(); break;
                case 's': result = Manager.SecondAverageTracker.Calculate(); break;
            }
            return Task.FromResult(result ?? throw new ArgumentException("Unknown period identifier", nameof(period)));
        }

        Task<IEnumerable<string>> ITweetLeadingHashtagProvider.GetLeadingAsyc(int count, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<string>>(Manager.LeadingHashtags.AsArray(x => x.Take(count)));
        }

        Task<IEnumerable<string>> ITweetLeadingDomainProvider.GetLeadingAsync(int count, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<string>>(Manager.LeadingDomainTracker.AsArray(x => x.Take(count)));
        }

        Task<IEnumerable<string>> ITweetLeadingEmojiProvider.GetLeadingAsync(int count, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<string>>(Manager.LeadingEmojiTracker.AsArray(x => x.Take(count)));
        }

        Task<decimal> ITweetWithEmojisPercentageProvider.GetPercentageAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(((decimal)Manager.ContainingEmojis / Manager.Total) * 100);
        }

        Task<decimal> ITweetWithPhotoUrlPercentageProvider.GetPercentageAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(((decimal)Manager.ContainingPhotoUrl / Manager.Total) * 100);
        }

        Task<decimal> ITweetWithUrlPercentageProvider.GetPercentageAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(((decimal)Manager.ContainingUrl / Manager.Total) * 100);
        }

        Task<int> ITweetTotalProvider.GetTotalAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Manager.Total);
        }
    }
}
