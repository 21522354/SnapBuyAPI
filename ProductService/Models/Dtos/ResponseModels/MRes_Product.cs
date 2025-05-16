namespace ProductService.Models.Dtos.ResponseModels
{
    public class MRes_Product
    {
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<MRes_ProductImage> ProductImages { get; set; }
        public ICollection<MRes_ProductVariant> ProductVariants { get; set; }
        public ICollection<string> ListTag { get; set; }
    }

    public class MRes_ProductRecommend
    {
        public int Id { get; set; }
        public string ProductString { get; set; }       
    }
}
