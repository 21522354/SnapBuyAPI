using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Dtos.ResponseModels
{
    public class MRes_VoucherUsage
    {
        public int Id { get; set; }
        public int VoucherId { get; set; }
        public Guid UserId { get; set; }
        public string OrderId { get; set; }
    }
}
