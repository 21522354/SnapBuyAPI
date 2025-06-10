using Bogus;
using OrderService.Models.Entities;
using OrderService.SyncDataService;
using System.Threading.Tasks;

namespace OrderService.Service
{
    public static class SeedData
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderDBContext>();
            var _productService = scope.ServiceProvider.GetRequiredService<IS_ProductDataClient>();

            if (context.Orders.Any()) return;

            var faker = new Faker("en");

            // Fixed user IDs
            var sellerIds = new[]
            {
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f91"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f92"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f93"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f94"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f95"),
            };

                    var buyerIds = new[]
                    {
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f96"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f97"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f98"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f99"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f90")
            };

            var statuses = new[] { "Pending", "Approved", "Success", "Failed" };

            for (int i = 0; i < sellerIds.Length; i++)
            {
                var sellerId = sellerIds[i];
                var buyerId = buyerIds[i];

                var productOfUserI = await _productService.GetProductBySeller2(sellerId);
                if (productOfUserI == null || productOfUserI.Count == 0) continue;

                for (int j = 0; j < 10; j++)
                {
                    // Random product
                    var product = faker.PickRandom(productOfUserI);
                    if (product == null || product.ProductVariants == null || product.ProductVariants.Count == 0) continue;

                    // Random variant
                    var variant = faker.PickRandom(product.ProductVariants);
                    var productImage = product.ProductImages?.FirstOrDefault()?.Url ?? "https://via.placeholder.com/150";

                    // Generate OrderItem
                    var quantity = faker.Random.Int(1, 5);
                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ProductImageUrl = productImage,
                        ProductNote = faker.Lorem.Sentence(),
                        ProductVariantId = variant.Id,
                        Quantity = 1,
                        UnitPrice = variant.Price,
                        IsReviewed = faker.Random.Bool()
                    };

                    // Generate Order
                    var order = new Order
                    {
                        Id = faker.Random.AlphaNumeric(10).ToUpper(),
                        BuyerId = buyerId,
                        SellerId = sellerId,
                        CreatedAt = faker.Date.Past(1),
                        PhoneNumber = faker.Phone.PhoneNumber("0#########"),
                        ShippingAddress = faker.Address.FullAddress(),
                        Status = faker.PickRandom(statuses),
                        TotalAmount = quantity * orderItem.UnitPrice,
                        OrderItems = new List<OrderItem> { orderItem },
                        VoucherUsages = new List<VoucherUsage>()
                    };

                    context.Orders.Add(order);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
