using Microsoft.EntityFrameworkCore;
using ProductService.Models.Entities;

namespace ProductService.Services
{
    public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductReviewImage> ProductReviewImages { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category
            modelBuilder.Entity<Category>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Category>()
                .HasOne(p => p.Parent)
                .WithMany(p => p.ChildCategories)
                .HasForeignKey(p => p.ParentId);

            // Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductVariants)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductImages)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);



            modelBuilder.Entity<ProductTag>()
                .HasOne(p => p.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<ProductTag>()
                .HasOne(p => p.Tag)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(p => p.TagId);

            modelBuilder.Entity<ProductReview>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ProductReview>()
                .HasMany(x => x.ProductReviewImages)
                .WithOne(x => x.ProductReview)
                .HasForeignKey(x => x.ProductReviewId);

            modelBuilder.Entity<ProductReviewImage>()
                .HasKey(x => x.Id);
                
           
        }
    }
}
