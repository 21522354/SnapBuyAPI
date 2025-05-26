using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.RequestModels
{
    public class MReq_ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
    }
    public class MReq_ProductVariantMin
    {
        public int ProductId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
    }
}
