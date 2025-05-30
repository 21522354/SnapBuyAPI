using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Dtos.RequestModels
{
    public class MReq_Voucher
    {

        public int Id { get; set; } 
        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required]
        public string Type { get; set; } // "percentage", "fixed", "free_shipping"

        [Required]
        public decimal Value { get; set; }
        public decimal? MinOrderValue { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsDisabled { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
