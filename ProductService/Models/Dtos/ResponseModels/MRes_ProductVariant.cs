using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.ResponseModels
{
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
