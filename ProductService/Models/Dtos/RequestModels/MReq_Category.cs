using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.RequestModels
{
    public class MReq_Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }        
        public int ParentId { get; set; }
    }
}
