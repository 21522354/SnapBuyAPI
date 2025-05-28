using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.ResponseModels
{
    public class MRes_ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public float StarNumber { get; set; }
        public string OrderId { get; set; }
        public string ProductNote { get; set; }
        public Guid UserId { get; set; }
        public string ReviewComment { get; set; }
        public List<string> ProductReviewImages { get; set; }
        public MRes_User User { get; set; }
    }
}
