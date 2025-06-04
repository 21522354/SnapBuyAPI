namespace OrderService.Models.Dtos.ResponseModels
{
    public class MRes_Notification
    {
        public Guid UserId { get; set; }
        public Guid UserInvoke { get; set; }
        public string Message { get; set; }
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        public string EventType { get; set; }
        public bool IsAlreadySeen { get; set; }
    }
}
