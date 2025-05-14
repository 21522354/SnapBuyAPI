using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductService.Services
{
    public static class SeedData
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductDBContext>();

            if (context.Categories.Any()) return;

            // ===== Categories =====
            var categories = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
                .RuleFor(c => c.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(c => c.CreatedAt, f => f.Date.Past())
                .Generate(15);

            // Set parent categories randomly
            for (int i = 0; i < categories.Count; i++)
            {
                if (i > 3)
                    categories[i].Parent = categories[new Random().Next(0, 4)];
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // ===== Tags =====
            var tags = new Faker<Tag>()
                .RuleFor(t => t.TagName, f => f.Commerce.ProductAdjective())
                .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                .RuleFor(t => t.CreatedAt, f => f.Date.Past())
                .Generate(20);

            context.Tags.AddRange(tags);
            context.SaveChanges();

            // ===== Products =====
            var products = new Faker<Product>()
                .RuleFor(p => p.SellerId, f => f.Random.Guid())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.BasePrice, f => f.Random.Decimal(10, 500))
                .RuleFor(p => p.Status, f => f.Random.Int(0, 2))
                .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
                .RuleFor(p => p.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past())
                .RuleFor(p => p.UpdatedAt, f => f.Date.Recent())
                .Generate(100);

            context.Products.AddRange(products);
            context.SaveChanges();

            // ===== ProductImages & ProductVariants & ProductTags =====
            var productImages = new List<ProductImage>();
            var productVariants = new List<ProductVariant>();
            var productTags = new List<ProductTag>();

            var rnd = new Random();

            foreach (var product in products)
            {
                // 3 images
                productImages.AddRange(new Faker<ProductImage>()
                    .RuleFor(i => i.ProductId, product.Id)
                    .RuleFor(i => i.Url, f => f.Image.PicsumUrl())
                    .RuleFor(i => i.IsThumbnail, f => false)
                    .RuleFor(i => i.CreatedAt, f => f.Date.Past())
                    .Generate(3));

                // 3 variants
                productVariants.AddRange(new Faker<ProductVariant>()
                    .RuleFor(v => v.ProductId, product.Id)
                    .RuleFor(v => v.Size, f => f.PickRandom(new[] { "S", "M", "L", "XL" }))
                    .RuleFor(v => v.Color, f => f.Commerce.Color())
                    .RuleFor(v => v.Price, f => f.Random.Decimal(10, 500))
                    .RuleFor(v => v.Status, f => f.Random.Int(0, 2))
                    .RuleFor(v => v.CreatedAt, f => f.Date.Past())
                    .Generate(3));

                // 5 tags
                var selectedTags = tags.OrderBy(_ => rnd.Next()).Take(5).ToList();
                foreach (var tag in selectedTags)
                {
                    productTags.Add(new ProductTag
                    {
                        ProductId = product.Id,
                        TagId = tag.Id
                    });
                }
            }

            context.ProductImages.AddRange(productImages);
            context.ProductVariants.AddRange(productVariants);
            context.ProductTags.AddRange(productTags);
            context.SaveChanges();
        }
    }
}

