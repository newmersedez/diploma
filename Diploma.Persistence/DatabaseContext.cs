using Diploma.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}