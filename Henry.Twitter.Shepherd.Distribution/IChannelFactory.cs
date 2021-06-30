using System.Threading.Channels;

namespace Henry.Twitter.Shepherd.Distribution
{
    interface IChannelFactory<TValue>
    {
        Channel<TValue> CreateChannel();
    }
}
