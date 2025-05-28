namespace ProductService.Models.Entities
{
    public class ProductReview
    {
        public int Id { get; set; } 
        public int ProductId { get; set; }
        public float StarNumber { get; set; }
        public string OrderId { get; set; }
        public string ProductNote { get; set; }
        public Guid UserId { get; set; }
        public string ReviewComment { get; set; }
        public ICollection<ProductReviewImage> ProductReviewImages { get; set; }
        public Product Product { get; set; }    
    }
}
