namespace ProductService.Models.Dtos.RequestModels
{
    public class MReq_ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public bool IsThumbnail { get; set; }
    }

    public class MReq_ProductImageCreate
    {
        public int ProductId { get; set; }
        public List<string> Urls { get; set; }
    }
}
