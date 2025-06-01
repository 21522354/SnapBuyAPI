namespace ChatService.Models.Entities
{
    public class ChatRoom
    {
        public int Id { get; set; }         
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
