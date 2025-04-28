
using Microsoft.EntityFrameworkCore;
using ProductService.Mapper;
using ProductService.Services;

namespace ProductService
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
            builder.Services.AddDbContext<ProductDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            builder.Services.AddScoped<IS_Category, S_Category>();
            builder.Services.AddScoped<IS_Product, S_Product>();
            builder.Services.AddScoped<IS_ProductImage, S_ProductImage>();
            builder.Services.AddScoped<IS_ProductVariant, S_ProductVariant>();

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
