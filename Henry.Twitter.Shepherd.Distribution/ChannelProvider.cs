using System.Threading.Channels;

namespace Henry.Twitter.Shepherd.Distribution
{
    class ChannelProvider<TValue>
    {
        public Channel<TValue> Distributor { get; }

        public ChannelProvider(IChannelFactory<TValue> factory)
        {
            Distributor = factory.CreateChannel();
        }
    }
}
