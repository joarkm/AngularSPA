using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AngularSPA.Extensions
{
    public static class SeedDataConfigurationExtensions
    {
        public static IConfigurationBuilder AddSeedConfiguration(this IConfigurationBuilder configurationBuilder)
        {
            var seedDataPath = "Data" + Path.DirectorySeparatorChar
                                + "SeedData" + Path.DirectorySeparatorChar;
            return configurationBuilder
                    .AddJsonFile(seedDataPath + "claim_types.json", optional: true, reloadOnChange: false);
        }
    }
}
