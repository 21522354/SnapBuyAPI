using Microsoft.EntityFrameworkCore;
using NotificationService.Common;
using NotificationService.Models.Dtos.Response;

namespace NotificationService.Services
{
    public interface IS_Notification
    {
        Task<ResponseData<List<MRes_Notification>>> GetAllNotiForUser(Guid userId);
    }
    public class S_Notification : IS_Notification
    {
        private readonly NotificationDBContext _context;

        public S_Notification(NotificationDBContext context)
        {
            _context = context;
        }
        public async Task<ResponseData<List<MRes_Notification>>> GetAllNotiForUser(Guid userId)
        {
            var res = new ResponseData<List<MRes_Notification>>();
            try
            {
                var notis = await _context.Notifications.Where(x => x.UserId == userId).OrderByDescending(x => x.Id).ToListAsync();
                var returnData = notis.Select(x => new MRes_Notification
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserInvoke = x.UserInvoke,
                    Message = x.Message,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    EventType = x.EventType,
                    IsAlreadySeen = x.IsAlreadySeen,
                }).ToList();
                res.result = 1;
                res.data = returnData;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }
    }
}
