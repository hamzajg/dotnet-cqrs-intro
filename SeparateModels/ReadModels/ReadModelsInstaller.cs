using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeparateModels.ReadModels.InMemory;

namespace SeparateModels.ReadModels
{
    public static class ReadModelsInstaller
    {
        public static IServiceCollection AddReadModels(this IServiceCollection services, IConfiguration configuration)
        {
            if (StorageSettings.IsInMemory(configuration))
                return services.AddInMemoryReadModels();

            return services.AddPostgresReadModels(configuration.GetConnectionString("DefaultConnection"));
        }

        private static IServiceCollection AddInMemoryReadModels(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryReadModelStore>();

            services.AddSingleton<IPolicyInfoDtoProjection, InMemoryPolicyInfoDtoProjection>();
            services.AddSingleton<IPolicyVersionDtoProjection, InMemoryPolicyVersionDtoProjection>();
            services.AddSingleton<IPolicyInfoDtoFinder, InMemoryPolicyInfoDtoFinder>();
            services.AddSingleton<IPolicyVersionDtoFinder, InMemoryPolicyVersionDtoFinder>();

            return services;
        }

        private static IServiceCollection AddPostgresReadModels(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton<IPolicyInfoDtoProjection>(new PolicyInfoDtoProjection(cnnString));
            services.AddSingleton<IPolicyVersionDtoProjection>(new PolicyVersionDtoProjection(cnnString));
            services.AddSingleton<IPolicyInfoDtoFinder>(new PolicyInfoDtoFinder(cnnString));
            services.AddSingleton<IPolicyVersionDtoFinder>(new PolicyVersionDtoFinder(cnnString));

            return services;
        }
    }
}
