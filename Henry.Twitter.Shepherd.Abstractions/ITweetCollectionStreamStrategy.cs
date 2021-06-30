using System.Collections.Generic;
using System.Threading;

namespace Henry.Twitter.Shepherd.Abstractions
{
    public interface ITweetCollectionStreamStrategy<TCollectable>
    {
        IAsyncEnumerable<TCollectable> StreamAsync(CancellationToken cancellationToken);
    }
}
