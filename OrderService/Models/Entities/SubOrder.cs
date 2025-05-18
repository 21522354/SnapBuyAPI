namespace OrderService.Models.Entities
{
    public class SubOrder
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid SellerId { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Order Order { get; set; }
        public ICollection<SubOrderItem> SubOrderItems { get; set; }
    }
}
