using System;

namespace AngularSPA.Data
{

    public class DbInitializer : IDbInitializer
    {
        private readonly IDbContext _dbContext;

        public DbInitializer(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool SeedContext()
        {
            throw new NotImplementedException();
        }
    }
}
