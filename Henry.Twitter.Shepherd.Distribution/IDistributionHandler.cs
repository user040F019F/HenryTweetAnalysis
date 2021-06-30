using System.Threading;
using System.Threading.Tasks;

namespace Henry.Twitter.Shepherd.Distribution
{
    interface IDistributionHandler<in TDistributed>
    {
        Task ProcessAsync(TDistributed distributed, CancellationToken cancellationToken);
    }
}
