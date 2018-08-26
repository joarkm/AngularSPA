using AngularSPA.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AngularSPA.Data
{
    public class DbContextFactory
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly Credentials _credentials;

        public DbContextFactory(
            DbContextOptions<ApplicationDbContext> dbContextOptions,
            Credentials credentials
        )
        {
            _dbContextOptions = dbContextOptions;
            _credentials = credentials;
        }

        public IDbContext CreateUserDbContext()
        {
            var userDbContext = new ApplicationDbContext(_dbContextOptions);
            var initializer = new UserDbInitializer(userDbContext, new PasswordHasher(), _credentials);
            initializer.SeedContext();
            return userDbContext;
        }
    }
}
