using OrderService.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Dtos.RequestModels
{
    public class MReq_VoucherUsage
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string OrderId { get; set; }
    }
}
