using System;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events.V2;

namespace Henry.Twitter.Shepherd.Invi
{
    public class InviV2SampleStreamProvider : IInviV2TweetStreamProvider
    {
        readonly ITwitterClient Client;

        public InviV2SampleStreamProvider(
            ITwitterClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task StreamAsync(EventHandler<TweetV2ReceivedEventArgs> handler, CancellationToken cancellationToken)
        {
            var stream = Client.StreamsV2.CreateSampleStream();
            stream.TweetReceived += handler;
            using var registration = cancellationToken.Register(() => stream.StopStream());
            await stream.StartAsync();
            stream.TweetReceived -= handler;
        }
    }
}
