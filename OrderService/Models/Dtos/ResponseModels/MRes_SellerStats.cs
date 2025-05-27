namespace OrderService.Models.Dtos.ResponseModels
{
    public class MRes_SellerStats
    {
        public int ProductCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalPurchases { get; set; }     
    }
}
