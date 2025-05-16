namespace CartService.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public List<CartItem> Items { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
