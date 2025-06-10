
using Microsoft.EntityFrameworkCore;
using OrderService.AsyncDataService;
using OrderService.Mapper;
using OrderService.Service;
using OrderService.SyncDataService;
using System.Threading.Tasks;

namespace OrderService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddDbContext<OrderDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });
            builder.Services.AddHttpClient<IS_UserDataClient, S_UserDataClient>();
            builder.Services.AddHttpClient<IS_ProductDataClient, S_ProductDataClient>();
            builder.Services.AddScoped<IS_Order, S_Order>();
            builder.Services.AddScoped<IS_Voucher, S_Voucher>();
            builder.Services.AddScoped<IS_VoucherUsage, S_VoucherUsage>();
            builder.Services.AddScoped<IMessageBusClient, MessageBusClient>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedData.SeedDatabase(services);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
