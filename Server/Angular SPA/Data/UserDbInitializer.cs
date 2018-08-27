﻿using AngularSPA.Helpers;
using AngularSPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AngularSPA.Data
{
    public class UserDbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserDbInitializer(
            ApplicationDbContext dbContext,
            IPasswordHasher passwordHasher,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
        }

        private bool SeedFromConfig()
        {
            var claimTypesSection = _configuration.GetSection(nameof(ClaimType));
            var claimTypes = claimTypesSection.Get<ClaimType[]>();
            
            // Read admin user credentials from appsettings
            var credentialsSection = _configuration.GetSection(nameof(Credentials));
            var adminUserCredentials = credentialsSection.GetSection("Admin").Get<Credentials>();


            return SeedEntities(claimTypes) && SeedAdminUser(adminUserCredentials);
        }

        public bool SeedEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _dbContext.Set<TEntity>().AddRange(entities);
            return Convert.ToBoolean(_dbContext.SaveChanges());
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
            
            return SeedRoles() && SeedRegularUser() && SeedFromConfig();
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

        private bool SeedAdminUser(Credentials credentials)
        {
            var adminRole = _dbContext.Role
                .FirstOrDefault(role => role.Name.Equals("Admin"));
            if (adminRole == null) return false;
            var adminUser = new User
            {
                UserName = credentials.UserName,
                Password = new Password
                {
                    Hash = _passwordHasher.CreatePasswordHash(credentials.Password)
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
