using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dtos.RequestModels
{
    public class MReq_UserNameImage
    {
        [Required]
        public Guid ID { get; set; }    
        public string Name { get; set; } 
        public string ImageUrl { get; set; }        
    }
}
