namespace NotificationService.Models.Dtos.Request
{
    public class MReq_Notification
    {
        public Guid UserId { get; set; }
        public Guid UserInvoke { get; set; }
        public string Message { get; set; }
        public Guid OrderId { get; set; }   
        public Guid ProductId { get; set; }
        public string EventType { get; set; }
    }
}
