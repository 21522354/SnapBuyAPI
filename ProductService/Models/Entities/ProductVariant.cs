namespace ProductService.Models.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
