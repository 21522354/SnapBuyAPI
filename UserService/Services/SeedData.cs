using Bogus;
using UserService.Models.Entities;

namespace UserService.Services
{
    public static class SeedData
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserDBContext>();

            if(context.Users.Any()) return;

            var faker = new Faker("en");

            // ===== Fixed User IDs =====
            var userIds = new[]
            {
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f91"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f92"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f93"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f94"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f95"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f96"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f97"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f98"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f99"),
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f90")
            };

            var users = new List<User>();
            var addresses = new List<UserAddresses>();

            int addressId = 1;

            int index = 0;

            foreach (var userId in userIds)
            {
                var user = new User
                {
                    ID = userId,
                    Name = faker.Name.FullName(),
                    ImageURL = faker.Internet.Avatar(),
                    UserName = faker.Internet.UserName(),
                    Email = "user" + (index + 1).ToString() + "@gmail.com",
                    Password = "123",
                    GoogleId = faker.Random.Guid().ToString(),
                    SELLER_MERCHANT_ID = "",
                    IsAdmin = false,  // admin vẫn random
                    IsPremium = index < 5,                    // 5 user đầu tiên là premium
                    LastProductId = faker.Random.Int(0, 100)
                };

                if (index == 2)
                {
                    user.SELLER_MERCHANT_ID = "MERCHANT_ID_123442122";
                }

                if (index == 4)
                {
                    user.SELLER_MERCHANT_ID = "MERCHANT_ID_123442211";
                }

                users.Add(user);

                addresses.AddRange(new Faker<UserAddresses>()
                    .RuleFor(a => a.Id, _ => addressId++)
                    .RuleFor(a => a.UserId, _ => userId)
                    .RuleFor(a => a.Address, f => f.Address.FullAddress())
                    .RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber())
                    .Generate(2));

                index++;
            }

            var admin = new User()
            {
                ID = Guid.NewGuid(),
                Name = "admin",
                ImageURL = faker.Internet.Avatar(),
                UserName = "admin",
                Email = "admin@gmail.com",
                Password = "admin",
                GoogleId = faker.Random.Guid().ToString(),
                SELLER_MERCHANT_ID = "",
                IsAdmin = true,
                LastProductId = 10,
            };
            context.Users.Add(admin);


            context.Users.AddRange(users);
            context.UserAddresses.AddRange(addresses);
            context.SaveChanges();
        }
    }
}
