using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.ResponseModels
{
    public class MRes_Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
