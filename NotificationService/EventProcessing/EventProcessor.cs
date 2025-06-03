using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using NotificationService.Models.Dtos.Request;
using System.Text.Json;

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

        private void NewOrderEvent(string message)
        {
            throw new NotImplementedException();
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
