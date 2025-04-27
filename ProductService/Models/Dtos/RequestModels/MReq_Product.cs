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
    }
}
