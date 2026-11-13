using Marten;
using Microsoft.Extensions.DependencyInjection;
using SeparateModels.Domain;

namespace SeparateModels.DataAccess.Marten
{
    public static class MartenInstaller
    {
        public static void AddMarten(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton<IDocumentStore>(CreateDocumentStore(cnnString));

            services.AddScoped<IDataStore, MartenDataStore>();
        }

        private static IDocumentStore CreateDocumentStore(string cn)
        {
            return DocumentStore.For(opts =>
            {
                opts.Connection(cn);

                opts.Schema.For<Policy>().Index(idx => idx.Number, index => index.IsUnique = true);
                opts.Schema.For<Offer>().Index(idx => idx.Number, index => index.IsUnique = true);
                opts.Schema.For<Product>().Index(idx => idx.Code, index => index.IsUnique = true);
            });
        }
    }
}
