using ChatService.Models.Entities;

namespace ChatService.Models.Dtos.RequestModel
{
    public class MReq_ChatMessage
    {
        public int Id { get; set; }     
        public int ChatRoomId { get; set; }
        public Guid UserSendId { get; set; }
        public DateTime SendDate { get; set; }
        public string? Message { get; set; }
        public string? MediaLink { get; set; }
        public string Type { get; set; }
    }

    public class MReq_SendText
    {
        public Guid UserSendId { get; set; }
        public Guid UserReceiveId { get; set; }
        public string Message { get; set; }     
    }

    public class MReq_SendImage
    {
        public Guid UserSendId { get; set; }
        public Guid UserReceiveId { get; set; }
        public string MediaLink { get; set; }       
    }

    public class MReq_SendVideo
    {
        public Guid UserSendId { get; set; }
        public Guid UserReceiveId { get; set; }
        public string MediaLink { get; set; }
    }
}
