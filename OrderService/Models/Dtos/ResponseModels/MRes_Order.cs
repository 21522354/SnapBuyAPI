using OrderService.Models.Entities;

namespace OrderService.Models.Dtos.ResponseModels
{
    public class MRes_Order
    {
        public string Id { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string Status { get; set; }
        public List<MRes_OrderItem> OrderItems { get; set; }
        public MRes_User Buyer { get; set; }
        public MRes_User Seller { get; set; }
    }
}
