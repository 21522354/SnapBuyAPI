namespace OrderService.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public Guid BuyerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public ICollection<SubOrder> SubOrders { get; set; }        
    }
}
