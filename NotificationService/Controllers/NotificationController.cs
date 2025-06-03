using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Services;

namespace NotificationService.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IS_Notification _s_Notification;

        public NotificationController(IS_Notification s_Notification)
        {
            _s_Notification = s_Notification;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllNotiForUser(Guid userId)
        {
            var res = await _s_Notification.GetAllNotiForUser(userId);
            return Ok(res);
        }
    }
}
