namespace OrderService.Models.Entities
{
    public class SubOrderItem
    {
        public int Id { get; set; }
        public int SubOrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductNote { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public SubOrder SubOrder { get; set; }  
    }
}
