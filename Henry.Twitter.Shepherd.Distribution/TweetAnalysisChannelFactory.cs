using Henry.Twitter.Analyzation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Henry.Twitter.Shepherd.Distribution
{
    class TweetAnalysisChannelFactory : IChannelFactory<TweetAnalysis>
    {
        public Channel<TweetAnalysis> CreateChannel()
        {
            return Channel.CreateUnbounded<TweetAnalysis>(
                new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false
                });
        }
    }
}
