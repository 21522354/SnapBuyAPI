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
}
