using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace AngularSPA.Data
{
    public class ContextFactory : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<DbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<DbContext>()
                .UseSqlite(_connection)
                .Options;
        }

        public DbContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions();
                using (var context = new DbContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new DbContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection == null) return;
            _connection.Dispose();
            _connection = null;
        }

    }
}
