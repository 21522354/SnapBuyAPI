
using Microsoft.EntityFrameworkCore;
using NotificationService.AsyncDataService;
using NotificationService.EventProcessing;
using NotificationService.Hubs;
using NotificationService.Services;

namespace NotificationService
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

            builder.Services.AddSignalR();
            builder.Services.AddDbContext<NotificationDBContext>(options =>
            {
                options.UseInMemoryDatabase("InMem");
            });

            builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
            builder.Services.AddScoped<IS_Notification, S_Notification>();
            builder.Services.AddHostedService<MessageBusSubscriber>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder => builder
                        .WithOrigins("http://localhost:5216")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

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

            app.UseCors("AllowFrontend");

            app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAll");

            app.Run();
        }
    }
}
