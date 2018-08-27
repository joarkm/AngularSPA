using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AngularSPA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = args == null ? BuildWebHost() : BuildWebHost(args);
            host.Run();
        }

        public static IWebHost BuildWebHost() =>
            new WebHostBuilder()
            .UseStartup<Startup>().Build();

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
