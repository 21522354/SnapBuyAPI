using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Entities
{
    public class VoucherUsage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int VoucherId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string OrderId { get; set; } 
        public DateTime UsedAt { get; set; }
        public Voucher Voucher { get; set; }
        public Order Order { get; set; }    
    }
}
