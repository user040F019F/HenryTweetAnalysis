using Henry.Twitter.Shepherd.Abstractions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using Tweetinvi.Events.V2;
using Tweetinvi.Models.V2;

namespace Henry.Twitter.Shepherd.Invi
{
    class InviStreamCollectionStrategy : ITweetCollectionStreamStrategy<TweetV2>
    {
        readonly IInviV2TweetStreamProvider StreamProvider;
        readonly Channel<TweetV2> Intermediary = Channel.CreateUnbounded<TweetV2>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = true
            });

        public InviStreamCollectionStrategy(IInviV2TweetStreamProvider streamProvider)
        {
            StreamProvider = streamProvider ?? throw new ArgumentNullException(nameof(streamProvider));
        }

        public async IAsyncEnumerable<TweetV2> StreamAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var task = StreamProvider.StreamAsync(HandleTweetAsync, cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
                yield return await Intermediary.Reader.ReadAsync(cancellationToken);
            await task;
        }

        async void HandleTweetAsync(object sender, TweetV2ReceivedEventArgs e)
        {
            if (e.Tweet != null) await Intermediary.Writer.WriteAsync(e.Tweet);
        }
    }
}
