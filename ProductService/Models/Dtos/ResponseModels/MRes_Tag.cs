using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.ResponseModels
{
    public class MRes_Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public int NumberOfProduct { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
