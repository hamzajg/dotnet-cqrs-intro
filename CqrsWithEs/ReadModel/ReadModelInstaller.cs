using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsWithEs.ReadModel
{
    public static class ReadModelInstaller
    {
        public static IServiceCollection AddReadModels(this IServiceCollection services, IConfiguration configuration)
        {
            if (StorageSettings.IsInMemory(configuration))
            {
                services.AddSingleton<IPolicyInfoDao, InMemoryPolicyInfoDao>();
                return services;
            }

            services.AddSingleton<IPolicyInfoDao>(
                new PolicyInfoDao(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
