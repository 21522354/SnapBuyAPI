using Microsoft.EntityFrameworkCore;
using NotificationService.Models.Entities;

namespace NotificationService.Services
{
    public class NotificationDBContext : DbContext
    {
        public NotificationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>()
                .HasKey(x => x.Id);
        }
    }
}
