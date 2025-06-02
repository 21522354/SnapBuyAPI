using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Dtos.RequestModels
{
    public class MReq_User
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class MReq_UserAddress
    {
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class MReq_UserMerchant
    {
        public Guid Id { get; set; }
        public string SELLER_MERCHANT_ID { get; set; }
    }
}
