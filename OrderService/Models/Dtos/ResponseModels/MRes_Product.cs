namespace OrderService.Models.Dtos.ResponseModels
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
    }

    public class MRes_Product2
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
        public List<MRes_ProductImage> ProductImages { get; set; }
        public List<MRes_ProductVariant> ProductVariants { get; set; }  
    }

    public class MRes_ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public bool IsThumbnail { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class MRes_ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
