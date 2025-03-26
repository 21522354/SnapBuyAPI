using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Entities
{
    public class User
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }     
        [Required]
        public string Password { get; set; }            
    }
}
