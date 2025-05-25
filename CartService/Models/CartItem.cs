namespace CartService.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductNote { get; set; }
        public int Quantity { get; set; }
    }
}