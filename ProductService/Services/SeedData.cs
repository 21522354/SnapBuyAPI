﻿using Bogus;
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

            var tagNames = new[]
            {
                "Eco-friendly", "Compact", "Durable", "Portable", "Stylish",
                "Luxury", "Affordable", "Limited", "Smart", "Classic",
                "Lightweight", "Premium", "Trendy", "Comfortable", "Innovative",
                "Organic", "Modern", "Handmade", "Efficient", "Custom"
            };
            var tags = tagNames.Select(name => new Tag
            {
                TagName = name,
                Description = new Faker().Lorem.Sentence(),
                CreatedAt = DateTime.UtcNow
            }).ToList();

            context.Tags.AddRange(tags);
            context.SaveChanges();

            var userIds = new[]
            {
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f91"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f92"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f93"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f94"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f95"),
            };

            // ===== Products =====
            var products = new Faker<Product>()
                .RuleFor(p => p.SellerId, f => userIds[f.Random.Int(0, 4)])
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.BasePrice, f => f.Random.Decimal(10, 500))
                .RuleFor(p => p.Status, f => f.Random.Int(0, 2))
                .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
                .RuleFor(p => p.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past())
                .RuleFor(p => p.UpdatedAt, f => f.Date.Recent())
                .RuleFor(p => p.Status, f => f.Random.Int(0, 1))
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

                // 4 variants
                // Lấy 2 màu và 2 size bất kỳ
                var sizes = new[] { "S", "M", "L", "XL" }.OrderBy(_ => Guid.NewGuid()).Take(2).ToArray();
                var colors = Enumerable.Range(0, 2)
                    .Select(_ => $"#{new Random().Next(0x1000000):X6}")
                    .ToArray();

                // Kết hợp tạo 4 variant (2 size x 2 color)
                foreach (var size in sizes)
                {
                    foreach (var color in colors)
                    {
                        productVariants.Add(new ProductVariant
                        {
                            ProductId = product.Id,
                            Size = size,
                            Color = color,
                            Price = new Random().Next(10, 500),
                            Status = new Random().Next(0, 3),
                            CreatedAt = DateTime.UtcNow.AddDays(-new Random().Next(1, 365))
                        });
                    }
                }

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

