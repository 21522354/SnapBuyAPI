namespace UserService.Models.Dtos.ResponseModels
{
    public class MRes_UserAddress
    {
        public int Id { get; set; }     
        public Guid UserId { get; set; }
        public string Address { get; set; }    
    }
}
