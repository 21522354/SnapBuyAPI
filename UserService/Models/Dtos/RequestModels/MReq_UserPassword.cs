using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dtos.RequestModels
{
    public class MReq_UserPassword
    {
        [Required]
        public Guid ID { get; set; }
        [Required]
        public string Password { get; set; }        
    }
}
