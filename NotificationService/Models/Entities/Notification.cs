using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace NotificationService.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; } 
        public Guid UserId { get; set; }
        public Guid UserInvoke { get; set; }
        public string Message { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string EventType { get; set; }
        public bool IsAlreadySeen { get; set; } 
    }
}
