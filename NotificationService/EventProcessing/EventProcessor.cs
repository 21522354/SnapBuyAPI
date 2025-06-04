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
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.NewOrder:
                    NewOrderEvent(message);
                    break;
                case EventType.ApproveOrder:
                    ApproveOrderEvent(message);
                    break;
                case EventType.CancelOrder:
                    CancelOrderEvent(message);
                    break;
                case EventType.SuccessOrder:
                    SuccessOrderEvent(message);
                    break;
                case EventType.NewRatingProduct:
                    NewRatingProductEvent(message);
                    break;
                default:
                    break;
            }
        }

        private void NewRatingProductEvent(string message)
        {
            throw new NotImplementedException();
        }

        private void SuccessOrderEvent(string message)
        {
            throw new NotImplementedException();
        }

        private void CancelOrderEvent(string message)
        {
            throw new NotImplementedException();
        }

        private void ApproveOrderEvent(string message)
        {
            throw new NotImplementedException();
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
                case "ApproveOrder":
                    return EventType.ApproveOrder;
                case "CancelOrder":
                    return EventType.CancelOrder;
                case "SuccessOrder":
                    return EventType.SuccessOrder;
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
            ApproveOrder,
            CancelOrder,
            SuccessOrder,
            NewRatingProduct,
            Undetermined,
        }
    }
}
