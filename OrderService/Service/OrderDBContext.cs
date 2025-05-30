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
        public DbSet<OrderItem> SubOrderItems { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherUsage> VoucherUsages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Order>()
                .HasMany(x => x.VoucherUsages)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
                
        }
    }
}
