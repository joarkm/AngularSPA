using AngularSPA.Helpers;
using AngularSPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AngularSPA.Data
{
    public class UserDbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Credentials _credentials;

        public UserDbInitializer(
            ApplicationDbContext dbContext,
            IPasswordHasher passwordHasher,
            Credentials credentials
        )
        {
            _credentials = credentials;
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
        }

        public bool SeedContext()
        {
            try
            {
                _dbContext.Database.EnsureCreated();
            }
            catch (Exception)
            {
                return false;
            }
            
            return SeedRoles() && SeedAdminUser() && SeedRegularUser();
        }

        private bool SeedRoles()
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

            _dbContext.Role.AddRange(roles);
            
            return Convert.ToBoolean(_dbContext.SaveChanges());
        }

        private bool SeedAdminUser()
        {
            var adminRole = _dbContext.Role
                .FirstOrDefault(role => role.Name.Equals("Admin"));
            if (adminRole == null) return false;
            var adminUser = new User
            {
                UserName = _credentials.UserName,
                Password = new Password
                {
                    Hash = _passwordHasher.CreatePasswordHash(_credentials.Password)
                },
                Role = adminRole
            };
            _dbContext.User.Add(adminUser);
            return Convert.ToBoolean(_dbContext.SaveChanges());
        }

        private bool SeedRegularUser()
        {
            var role = _dbContext.Role
                .FirstOrDefault(rol => rol.Name.Equals("Regular User"));
            if (role == null) return false;
            var user = new User
            {
                UserName = "regular",
                Password = new Password
                {
                    Hash = _passwordHasher.CreatePasswordHash("default")
                },
                Role = role
            };
            _dbContext.User.Add(user);
            return Convert.ToBoolean(_dbContext.SaveChanges());
        }
    }
}
