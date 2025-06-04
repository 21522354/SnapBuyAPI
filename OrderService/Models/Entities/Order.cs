namespace OrderService.Models.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; } 
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } 
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<VoucherUsage> VoucherUsages { get; set; }        
    }
}
