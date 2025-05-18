using Microsoft.EntityFrameworkCore;
using OrderService.Models.Entities;

namespace OrderService.Service
{
    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<SubOrder> SubOrders { get; set; }          
        public DbSet<SubOrderItem> SubOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.SubOrders)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            modelBuilder.Entity<SubOrder>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<SubOrder>()
                .HasMany(x => x.SubOrderItems)
                .WithOne(x => x.SubOrder)
                .HasForeignKey(x => x.SubOrderId);

            modelBuilder.Entity<SubOrderItem>()
                .HasKey(x => x.Id);
        }
    }
}
