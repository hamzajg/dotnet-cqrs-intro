using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeparateModels.Domain;

namespace SeparateModels.DataAccess.InMemory
{
    /// <summary>
    /// Dependency-free write-side store used by the InMemory storage profile, so the
    /// application runs without a PostgreSQL server. State lives as long as the process.
    /// </summary>
    public class InMemoryDataStore : IDataStore
    {
        public IProductRepository Products { get; }
        public IOfferRepository Offers { get; }
        public IPolicyRepository Policies { get; }

        public InMemoryDataStore()
        {
            Products = new InMemoryProductRepository();
            Offers = new InMemoryOfferRepository();
            Policies = new InMemoryPolicyRepository();
        }

        public Task CommitChanges() => Task.CompletedTask;
    }

    public class InMemoryProductRepository : IProductRepository
    {
        private readonly ConcurrentDictionary<string, Product> products = new();

        public void Add(Product product) => products[product.Code] = product;

        public Task<Product> WithCode(string code)
        {
            products.TryGetValue(code, out var product);
            return Task.FromResult(product);
        }

        public Task<IReadOnlyList<Product>> All() =>
            Task.FromResult<IReadOnlyList<Product>>(products.Values.ToList());
    }

    public class InMemoryOfferRepository : IOfferRepository
    {
        private readonly ConcurrentDictionary<string, Offer> offers = new();

        public void Add(Offer offer) => offers[offer.Number] = offer;

        public Task<Offer> WithNumber(string number)
        {
            offers.TryGetValue(number, out var offer);
            return Task.FromResult(offer);
        }

        public Task<IReadOnlyList<Offer>> All() =>
            Task.FromResult<IReadOnlyList<Offer>>(offers.Values.ToList());
    }

    public class InMemoryPolicyRepository : IPolicyRepository
    {
        private readonly ConcurrentDictionary<string, Policy> policies = new();

        public void Add(Policy policy) => policies[policy.Number] = policy;

        public Task<Policy> WithNumber(string number)
        {
            policies.TryGetValue(number, out var policy);
            return Task.FromResult(policy);
        }
    }
}
