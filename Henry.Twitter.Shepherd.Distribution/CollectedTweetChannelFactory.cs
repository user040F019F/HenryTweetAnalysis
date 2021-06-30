using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Henry.Twitter.Shepherd.Distribution
{
    class CollectedTweetChannelFactory<TCollected> : IChannelFactory<TCollected>
    {
        public Channel<TCollected> CreateChannel()
        {
            return Channel.CreateUnbounded<TCollected>(
                new UnboundedChannelOptions
                {
                    SingleWriter = true,
                    SingleReader = false
                });
        }
    }
}
