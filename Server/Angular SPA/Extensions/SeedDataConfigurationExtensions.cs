using Microsoft.Extensions.Configuration;
using System.IO;

namespace AngularSPA.Extensions
{
    public static class SeedDataConfigurationExtensions
    {
        public static IConfigurationBuilder AddSeedConfiguration(this IConfigurationBuilder configurationBuilder)
        {
            var seedDataPath = "Data" + Path.DirectorySeparatorChar
                                + "SeedData" + Path.DirectorySeparatorChar;
            return configurationBuilder
                    .AddJsonFile(seedDataPath + "claim_types.json", optional: true, reloadOnChange: false)
                    .AddJsonFile(seedDataPath + "roles.json", optional: true, reloadOnChange: false)
                    .AddJsonFile(seedDataPath + "users.json", optional: true, reloadOnChange: false);
        }
    }
}
