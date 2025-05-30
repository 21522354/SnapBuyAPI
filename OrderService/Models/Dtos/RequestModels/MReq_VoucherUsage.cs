using OrderService.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Dtos.RequestModels
{
    public class MReq_VoucherUsage
    {
        public int Id { get; set; }
        [Required]
        public int VoucherId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string OrderId { get; set; }
    }
}
