using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UserService.Models.Entities
{
    public class User
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string GoogleId { get; set; } = string.Empty;
    }
}
