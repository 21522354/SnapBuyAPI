using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dtos.RequestModels
{
    public class MReq_UserLoginGoogle
    {
        [Required]
        public string GoogleId { get; set; }
        [Required]
        public string Email { get; set; }       
    }
}
