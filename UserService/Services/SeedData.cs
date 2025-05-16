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
                Guid.Parse("5624994f-3a1a-4fa0-83ec-529ec3530f95")
            };

            var users = new List<User>();
            var addresses = new List<UserAddresses>();

            int addressId = 1;

            foreach (var userId in userIds)
            {
                var user = new User
                {
                    ID = userId,
                    Name = faker.Name.FullName(),
                    ImageURL = faker.Internet.Avatar(),
                    UserName = faker.Internet.UserName(),
                    Email = faker.Internet.Email(),
                    Password = faker.Internet.Password(),
                    GoogleId = faker.Random.Guid().ToString(),
                    IsAdmin = faker.Random.Bool(0.2f),       // 20% là admin
                    IsPremium = faker.Random.Bool(0.3f),     // 30% là premium
                    LastProductId = faker.Random.Int(0, 100)
                };

                users.Add(user);

                // Thêm 2 địa chỉ cho mỗi user
                addresses.AddRange(new Faker<UserAddresses>()
                    .RuleFor(a => a.Id, _ => addressId++)
                    .RuleFor(a => a.UserId, _ => userId)
                    .RuleFor(a => a.Address, f => f.Address.FullAddress())
                    .Generate(2));
            }

            context.Users.AddRange(users);
            context.UserAddresses.AddRange(addresses);
            context.SaveChanges();
        }
    }
}
