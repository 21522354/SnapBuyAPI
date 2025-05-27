namespace OrderService.Models.Dtos.ResponseModels
{
    public class MRes_BuyerStats
    {
        public int PurchaseCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalOrders { get; set; }    
    }
}
