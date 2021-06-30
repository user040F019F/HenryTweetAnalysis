using System;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Events.V2;

namespace Henry.Twitter.Shepherd.Invi
{
    public interface IInviV2TweetStreamProvider
    {
        Task StreamAsync(EventHandler<TweetV2ReceivedEventArgs> handler, CancellationToken cancellationToken);
    }
}
