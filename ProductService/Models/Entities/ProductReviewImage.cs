namespace ProductService.Models.Entities
{
    public class ProductReviewImage
    {
        public int Id { get; set; }
        public int ProductReviewId { get; set; }
        public string Url { get; set; }
        public ProductReview ProductReview { get; set; }    
    }
}
