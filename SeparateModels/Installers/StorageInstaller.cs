using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeparateModels.DataAccess.InMemory;
using SeparateModels.DataAccess.Marten;
using SeparateModels.Domain;

namespace SeparateModels.Installers
{
    public static class StorageInstaller
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
        {
            if (StorageSettings.IsInMemory(configuration))
            {
                services.AddSingleton<IDataStore, InMemoryDataStore>();
                return services;
            }

            services.AddMarten(configuration.GetConnectionString("DefaultConnection"));
            return services;
        }
    }
}
