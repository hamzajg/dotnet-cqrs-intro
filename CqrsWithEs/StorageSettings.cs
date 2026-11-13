using System;
using Microsoft.Extensions.Configuration;

namespace CqrsWithEs
{
    /// <summary>
    /// Selects the read-model store. The write side is always in memory here; `Storage:Provider`
    /// is `InMemory` (the default) or `Postgres`, which needs the DefaultConnection server.
    /// </summary>
    public static class StorageSettings
    {
        public const string InMemory = "InMemory";
        public const string Postgres = "Postgres";

        public static string Provider(IConfiguration configuration) =>
            configuration["Storage:Provider"] ?? InMemory;

        public static bool IsInMemory(IConfiguration configuration) =>
            string.Equals(Provider(configuration), InMemory, StringComparison.OrdinalIgnoreCase);
    }
}
