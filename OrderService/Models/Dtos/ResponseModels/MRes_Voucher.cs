using System.ComponentModel.DataAnnotations;

namespace OrderService.Models.Dtos.ResponseModels
{
    public class MRes_Voucher
    {
        public int Id { get; set; }     
        public string Code { get; set; }
        public string Type { get; set; } // "percentage", "fixed"
        public decimal Value { get; set; }
        public decimal? MinOrderValue { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool CanUse { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
