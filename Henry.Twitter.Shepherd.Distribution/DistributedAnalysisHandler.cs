using Henry.Twitter.Analyzation.Abstractions;
using Henry.Twitter.Analyzation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd.Distribution
{
    class DistributedAnalysisHandler : IDistributionHandler<TweetAnalysis>
    {
        readonly IEnumerable<ITweetAnalysisHandler> Handlers;

        public DistributedAnalysisHandler(
            IEnumerable<ITweetAnalysisHandler> handlers)
        {
            Handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        }

        public Task ProcessAsync(TweetAnalysis analysis, CancellationToken cancellationToken) =>
            /// It'd be nice to use DDD and an event broker to distribute this to
            /// external microservices to track different data points independently,
            /// but for now, just simulate it with different threads...
            Task.WhenAll(Handlers.Select(x => x.HandleAsync(analysis, cancellationToken)));
    }
}
