using System;
using Microsoft.Extensions.Configuration;

namespace SeparateModels
{
    /// <summary>
    /// Selects the persistence profile. `Storage:Provider` is `InMemory` (the default) or
    /// `Marten`, which needs a PostgreSQL server and its connection string.
    /// </summary>
    public static class StorageSettings
    {
        public const string InMemory = "InMemory";
        public const string Marten = "Marten";

        public static string Provider(IConfiguration configuration) =>
            configuration["Storage:Provider"] ?? InMemory;

        public static bool IsInMemory(IConfiguration configuration) =>
            string.Equals(Provider(configuration), InMemory, StringComparison.OrdinalIgnoreCase);
    }
}
