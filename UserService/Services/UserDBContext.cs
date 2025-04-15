using Microsoft.EntityFrameworkCore;
using UserService.Models.Entities;

namespace UserService.Services
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddresses> UserAddresses { get; set; }     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(p => p.UserAddresses)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
        }
    }
}
