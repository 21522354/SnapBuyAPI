
using Microsoft.EntityFrameworkCore;
using OrderService.Mapper;
using OrderService.Service;
using OrderService.SyncDataService;

namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
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
            builder.Services.AddScoped<IS_Order, S_Order>();

            var app = builder.Build();

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
