using Diploma.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPublicKey> UserPublicKeys { get; set; }
        public DbSet<UserPrivateKey> UserPrivateKeys { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}