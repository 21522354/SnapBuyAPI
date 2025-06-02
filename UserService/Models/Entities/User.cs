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
        public bool IsAdmin { get; set; } = false;
        public bool IsPremium { get; set; } = false;
        public bool IsBanned { get; set; } = false;
        public int LastProductId { get; set; } = 0;
        public string SELLER_MERCHANT_ID { get; set; }  
        public ICollection<UserAddresses> UserAddresses { get; set; }
    }
}
