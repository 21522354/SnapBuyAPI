using OrderService.Models.Dtos.ResponseModels;

namespace OrderService.AsyncDataService
{
    public interface IMessageBusClient
    {
        Task PublishNewNotification(MRes_Notification notification);
    }
}
