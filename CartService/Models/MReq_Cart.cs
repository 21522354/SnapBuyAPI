namespace CartService.Models
{
    public class MReq_Cart
    {
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductNote { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }  
    }
}
