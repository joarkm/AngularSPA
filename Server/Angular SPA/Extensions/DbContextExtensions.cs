using AngularSPA.Data;
using System;
using System.Linq;

namespace AngularSPA.Extensions
{
    public static class DbContextExtensions
    {
        public static bool IsSeeded(this ApplicationDbContext dbContext)
        {
            bool created;
            try
            {
                created = dbContext.Database.EnsureCreated();
            }
            catch (Exception)
            {
                return dbContext.User.Any();
            }

            return !created;
            
        }
    }
}
