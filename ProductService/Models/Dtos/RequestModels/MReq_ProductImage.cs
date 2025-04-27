namespace ProductService.Models.Dtos.RequestModels
{
    public class MReq_ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public bool IsThumbnail { get; set; }
    }
}
