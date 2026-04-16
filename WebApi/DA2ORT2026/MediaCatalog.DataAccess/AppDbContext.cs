
using MediaCatalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;

namespace MediaCatalog.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            if (!options.Extensions.OfType<SqliteOptionsExtension>().Any())
            {
                Database.Migrate();
            }
        }
    }
}
