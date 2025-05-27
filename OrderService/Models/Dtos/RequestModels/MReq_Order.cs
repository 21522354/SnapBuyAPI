using OrderService.Models.Entities;

namespace OrderService.Models.Dtos.RequestModels
{
    public class MReq_Order
    {
        public string Id { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public List<MReq_OrderItem> OrderItems { get; set; }
    }
}
