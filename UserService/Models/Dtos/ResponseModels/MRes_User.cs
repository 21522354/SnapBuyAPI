using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dtos.ResponseModels
{
    public class MRes_User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPremium { get; set; }
        public bool IsBanned { get; set; }
        public string SELLER_MERCHANT_ID { get; set; }
        public int LastProductId { get; set; }
    }
}
