using AngularSPA.Helpers;
using AngularSPA.Models;
using AngularSPA.Models.SeedModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularSPA.Extensions;

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

            // Seed ClaimType entities
            SeedEntities(claimTypes);

            var claimsSection = _configuration.GetSection(nameof(Claim));
            var claimsSeedModels = claimsSection.Get<ClaimSeedModel[]>();

            foreach (var claimsSeedModel in claimsSeedModels)
            {
                var claimType = _dbContext.ClaimType
                    .Single(ct => ct.Name.Equals(claimsSeedModel.ClaimType));
                _dbContext.Claim.Add(new Claim
                {
                    ClaimType = claimType,
                    ClaimValue = claimsSeedModel.ClaimValue
                });
            }

            // Seed Claim entities
            _dbContext.SaveChanges();

            // Read roles from config
            var rolesSection = _configuration.GetSection(nameof(Role));
            var roleSeedModels = rolesSection.Get<RoleSeedModel[]>();

            var roles = roleSeedModels
                .Select(rol => new Role
                {
                    Name = rol.RoleName,
                    NormalizedName = rol.RoleName.ToUpperInvariant(),
                    Description = rol.RoleDescription
                })
                .ToArray();

            // Seed Role entities
            SeedEntities(roles);

            // Assign claims to roles
            //foreach (var roleSeedModel in roleSeedModels)
            //{
            //    var claimSeedModels = roleSeedModel.Claim;
            //    // DbContext is not thread safe, so this does not work...
            //    Parallel.ForEach(claimSeedModels, (claimSeedModel) =>
            //    {
            //        var claim = _dbContext.Claim
            //            .FirstOrDefault(cl =>
            //                cl.ClaimType.Name.Equals(claimSeedModel.ClaimType)
            //                && cl.ClaimValue.Equals(claimSeedModel.ClaimValue));
            //        var role = _dbContext.Role.Single(rol => rol.Name.Equals(roleSeedModel.RoleName));
            //        var roleClaim = new RoleClaim
            //        {
            //            Role = role,
            //            Claim = claim
            //        };
            //        _dbContext.RoleClaim.Add(roleClaim);
            //    });

            //}
            //// Seed RoleClaim entities
            //_dbContext.SaveChanges();

            foreach (var roleSeedModel in roleSeedModels)
            {
                foreach (var claimSeedModel in roleSeedModel.Claim)
                {
                    var claim = _dbContext.Claim
                        .FirstOrDefault(cl =>
                            cl.ClaimType.Name.Equals(claimSeedModel.ClaimType)
                            && cl.ClaimValue.Equals(claimSeedModel.ClaimValue));
                    var role = _dbContext.Role.Single(rol => rol.Name.Equals(roleSeedModel.RoleName));
                    var roleClaim = new RoleClaim
                    {
                        Role = role,
                        Claim = claim
                    };
                    _dbContext.RoleClaim.Add(roleClaim);
                }
            }
            _dbContext.SaveChanges();

            // Read roles from config
            var usersSection = _configuration.GetSection(nameof(User));
            var usersSeedModels = usersSection.Get<UserSeedModel[]>();

            var users = usersSeedModels
                .Select(usr => new User
                {
                    UserName = usr.UserName,
                    Role = _dbContext.Role.Single(rol => rol.Name.Equals(usr.Role)),
                    Password = new Password
                    {
                        Hash = _passwordHasher.CreatePasswordHash(usr.Password)
                    },
                    Person = usr.Person == null ? null : new Person
                    {
                        GivenName = usr.Person.GivenName,
                        Surname = usr.Person.SurName
                    },
                })
                .ToArray();

            // Seed User entities
            SeedEntities(users);

            // Read admin user credentials from appsettings
            var credentialsSection = _configuration.GetSection(nameof(Credentials));
            var adminUserCredentials = credentialsSection.GetSection("Admin").Get<Credentials>();

            return true;
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
            
            return SeedFromConfig();
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

        private bool SeedUser(Credentials credentials, Role role)
        {
            var user = new User
            {
                UserName = credentials.UserName,
                Password = new Password
                {
                    Hash = _passwordHasher.CreatePasswordHash(credentials.Password)
                },
                Role = role
            };
            _dbContext.User.Add(user);
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
