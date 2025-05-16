using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.ResponseModels
{
    public class MRes_Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int ParentId { get; set; }
        public int NumberOfProduct { get; set; }        
        public DateTime CreatedAt { get; set; }
    }
}
