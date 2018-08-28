using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Auth;
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
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddSeedConfiguration()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });
            
            var id = string.Format("{0}.db", Guid.NewGuid().ToString());

            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = id,
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared
            };

            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();
            connection.EnableExtensions();

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            services.TryAddSingleton(ctx => new UserDbInitializer(
                    new ApplicationDbContext(dbContextOptions),
                    new PasswordHasher(),
                    Configuration
            ));

            var dbContextFactory = new DbContextFactory(dbContextOptions, Configuration);
            
            services.TryAddScoped<IDbContext>(ctx => dbContextFactory.CreateUserDbContext());
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ISignInManager, SignInManager>();
            services.AddJwt();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    var dbContext = serviceProvider.GetService<IDbContext>();
                    var appDbContext = dbContext as ApplicationDbContext;
                    var dbInitializer = serviceProvider.GetService<UserDbInitializer>();

                    if (!appDbContext.IsSeeded())
                    {
                        dbInitializer.SeedContext();
                    }
                }
            }

            // IMPORTANT! UseAuthentication() must be called before UseMvc()

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
