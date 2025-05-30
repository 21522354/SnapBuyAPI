namespace UserService.Models.Entities
{
    public class UserAddresses
    {
        public int Id { get; set; }     
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string PhoneNumber { get; set; } 
        public string Address { get; set; }     
    }
}
