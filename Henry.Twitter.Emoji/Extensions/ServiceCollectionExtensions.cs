using Henry.Twitter.Analyzation.Abstractions;
using Henry.Twitter.Emoji;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultEmojiScanner(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<IEmojiScanner, Scanner>();
        }
    }
}
