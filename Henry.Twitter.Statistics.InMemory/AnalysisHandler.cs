using Henry.Twitter.Analyzation.Abstractions;
using Henry.Twitter.Analyzation.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Statistics.InMemory
{
    class AnalysisHandler : ITweetAnalysisHandler
    {
        readonly StatisticsManager Manager;

        public AnalysisHandler(
            StatisticsManager manager)
        {
            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        public Task HandleAsync(TweetAnalysis analysis, CancellationToken cancellationToken)
        {
            Manager.Handle(analysis);
            return Task.CompletedTask;
        }
    }
}
