using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.RequestModels
{
    public class MReq_Product
    {
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
    }

    public class MReq_ProductDetail
    {
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public List<string> ProductImages { get; set; }
        public List<MReq_ProductVariantMin> ProductVariants { get; set; }
        public List<string> Tags { get; set; }
    }
}
