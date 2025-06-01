namespace ChatService.Models.Dtos.ResponseModel
{
    public class MRes_ChatRoom
    {
        public int Id { get; set; }  
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }
}
