using ChattingApp.Foundation.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChattingApp.Foundation.Contexts
{
    public class ChattingContext : DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ChattingContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Message>()
                .HasOne(s => s.Sender)
                .WithMany(r => r.MessagesReceive)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(r => r.Recipient)
                .WithMany(s => s.MessagesSent)
                .HasForeignKey(r => r.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
