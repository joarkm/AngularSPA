using AngularSPA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace AngularSPA.Data
{
    public interface IDbContext: IDisposable
    {
        DbSet<Claim> Claim { get; set; }
        DbSet<ClaimType> ClaimType { get; set; }
        DbSet<Password> Password { get; set; }
        DbSet<Person> Person { get; set; }
        DbSet<Role> Role { get; set; }
        DbSet<RoleClaim> RoleClaim { get; set; }
        DbSet<User> User { get; set; }
        DbSet<UserLogin> UserLogin { get; set; }
        DbSet<UserToken> UserToken { get; set; }

        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
    }
}
