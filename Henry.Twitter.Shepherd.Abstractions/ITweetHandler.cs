using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd.Abstractions
{
    public interface ITweetHandler<in TCollected>
    {
        Task HandleAsync(TCollected tweet, CancellationToken cancellationToken = default);
    }
}
