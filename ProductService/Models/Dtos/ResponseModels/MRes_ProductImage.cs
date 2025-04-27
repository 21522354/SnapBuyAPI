namespace ProductService.Models.Dtos.ResponseModels
{
    public class MRes_ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public bool IsThumbnail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
