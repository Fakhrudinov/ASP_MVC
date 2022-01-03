using Microsoft.EntityFrameworkCore;
using MVC_Messenger.Models;

namespace MVC_Messenger.Data
{
    public class MessengerContext : DbContext
    {
        public MessengerContext(DbContextOptions<MessengerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Email> Emails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithOne(d => d.Role)
                .HasForeignKey(e => e.RoleId);
        }
    }
}
