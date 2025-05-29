namespace OrderService.Models.Dtos.ResponseModels
{
    public class MRes_OrderItem
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductNote { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public bool IsReviewed { get; set; }    
        public decimal UnitPrice { get; set; }
    }
}
