using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Data;
using AngularSPA.Extensions;
using AngularSPA.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ApplicationDbContext = AngularSPA.Data.ApplicationDbContext;

namespace AngularSPA
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddSeedConfiguration()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var id = string.Format("{0}.db", Guid.NewGuid().ToString());

            var builder = new SqliteConnectionStringBuilder()
            {
                DataSource = id,
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared
            };

            var connection = new SqliteConnection(builder.ConnectionString);
            connection.Open();
            connection.EnableExtensions();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContextFactory = new DbContextFactory(dbContextOptions, Configuration);
            
            services.TryAddScoped<IDbContext>(ctx => dbContextFactory.CreateUserDbContext());
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
