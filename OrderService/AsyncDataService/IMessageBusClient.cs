namespace OrderService.AsyncDataService
{
    public interface IMessageBusClient
    {
        Task PublishNewNotification(object notificationReadDTO);
    }
}
