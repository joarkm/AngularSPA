using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Models;

namespace AngularSPA.Data
{
    public class DbInitializer
    {
        private readonly IDbContext _dbContext;

        public DbInitializer(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IDbContext> SeedContext()
        {
            return await SeedRoles();
        }

        public async Task<IDbContext> SeedRoles()
        {
            var roles = new List<Role>
            {
                new Role()
                {
                    Name = "Super Admin"
                },
                new Role()
                {
                    Name = "Admin"
                },
                new Role()
                {
                    Name = "Regular User"
                }
            };
            foreach (var role in roles)
            {
                role.NormalizedName = role.Name.ToUpperInvariant();
                role.Description = $"{role.Name} role";
            }

            await _dbContext.Role.AddRangeAsync(roles);
            _dbContext.SaveChanges();
            return _dbContext;
        }
    }
}
