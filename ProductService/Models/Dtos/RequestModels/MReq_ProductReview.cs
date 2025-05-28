using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.RequestModels
{
    public class MReq_ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public float StarNumber { get; set; }
        public string OrderId { get; set; }
        public string ProductNote { get; set; }
        public Guid UserId { get; set; }
        public string ReviewComment { get; set; }
        public List<string> ProductReviewImages { get; set; }
    }
}
