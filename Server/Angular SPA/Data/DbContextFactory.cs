using AngularSPA.Extensions;
using AngularSPA.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AngularSPA.Data
{
    public class DbContextFactory
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IConfiguration _configuration;

        public DbContextFactory(
            DbContextOptions<ApplicationDbContext> dbContextOptions,
            IConfiguration configuration
        )
        {
            _dbContextOptions = dbContextOptions;
            _configuration = configuration;
        }

        public IDbContext CreateUserDbContext()
        {
            var userDbContext = new ApplicationDbContext(_dbContextOptions);
            var initializer = new UserDbInitializer(userDbContext, new PasswordHasher(), _configuration);
            if(!userDbContext.IsSeeded())
                initializer.SeedContext();
            return userDbContext;
        }
    }
}
