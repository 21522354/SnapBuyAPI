﻿namespace ChatService.Models.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public virtual ChatRoom ChatRoom { get; set; }
        public Guid UserSendId { get; set; }
        public DateTime SendDate { get; set; }
        public string? Message { get; set; }
        public string? MediaLink { get; set; }
        public string Type { get; set; }
    }
}
