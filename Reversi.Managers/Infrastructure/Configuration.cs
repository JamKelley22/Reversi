using Microsoft.Extensions.DependencyInjection;
using Reversi.Interfaces.Managers;

namespace ReversiManagers
{
    public static class Configuration
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddTransient<IReversiBoardManager, ReversiBoardManager>();
            return services;
        }
    }
}
