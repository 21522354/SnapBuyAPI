using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using NotificationService.Models.Dtos.Request;
using NotificationService.Models.Entities;
using NotificationService.Services;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IHubContext<NotificationHub> hubContext)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hubContext = hubContext;
        }
        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.NewOrder:
                    await NewOrderEvent(message);
                    break;
                case EventType.ChangeStatus:
                    await ChangeStatusEvent(message);
                    break;
                default:
                    break;
            }
        }

        private async Task ChangeStatusEvent(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<NotificationDBContext>();
                var notiDto = JsonSerializer.Deserialize<MReq_Notification>(message);
                try
                {
                    var noti = new Notification
                    {
                        UserId = notiDto.UserId,
                        UserInvoke = notiDto.UserInvoke,
                        Message = notiDto.Message,
                        OrderId = notiDto.OrderId,
                        ProductId = notiDto.ProductId,
                        EventType = notiDto.EventType,
                        IsAlreadySeen = false
                    };
                    _context.Notifications.Add(noti);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("NewNoti", noti.UserId.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
                Console.WriteLine(message);
            }
        }

        private async Task NewRatingProductEvent(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<NotificationDBContext>();
                var notiDto = JsonSerializer.Deserialize<MReq_Notification>(message);
                try
                {
                    var noti = new Notification
                    {
                        UserId = notiDto.UserId,
                        UserInvoke = notiDto.UserInvoke,
                        Message = notiDto.Message,
                        OrderId = notiDto.OrderId,
                        ProductId = notiDto.ProductId,
                        EventType = notiDto.EventType,
                        IsAlreadySeen = false
                    };
                    _context.Notifications.Add(noti);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("NewNoti", noti.UserId.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
                Console.WriteLine(message);
            }
        }

        private async Task NewOrderEvent(string message)
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<NotificationDBContext>();
                var notiDto = JsonSerializer.Deserialize<MReq_Notification>(message);
                try
                {
                    var noti = new Notification
                    {
                        UserId = notiDto.UserId,
                        UserInvoke = notiDto.UserInvoke,
                        Message = notiDto.Message,
                        OrderId = notiDto.OrderId,
                        ProductId = notiDto.ProductId,
                        EventType = notiDto.EventType,
                        IsAlreadySeen = false
                    };
                    _context.Notifications.Add(noti);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("NewNoti", noti.UserId.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
                Console.WriteLine(message);
            }
            
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<MReq_Notification>(notificationMessage);
            switch (eventType.EventType)
            {
                case "NewOrder":
                    return EventType.NewOrder;
                case "ChangeStatus":
                    return EventType.ChangeStatus;
                case "NewRatingProduct":
                    return EventType.NewRatingProduct;
                default:
                    Console.WriteLine("--> Could not determine Event type");
                    return EventType.Undetermined;
            }
        }

        enum EventType
        {
            NewOrder,
            ChangeStatus,
            NewRatingProduct,
            Undetermined
        }
    }
}
