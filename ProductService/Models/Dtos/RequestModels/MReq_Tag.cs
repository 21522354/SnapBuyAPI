using ProductService.Models.Entities;

namespace ProductService.Models.Dtos.RequestModels
{
    public class MReq_Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
    }
}
