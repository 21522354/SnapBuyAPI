using ChatService.Models.Dtos.RequestModel;
using ChatService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IS_ChatRoom _s_ChatRoom;

        public ChatController(IS_ChatRoom s_ChatRoom)
        {
            _s_ChatRoom = s_ChatRoom;
        }

        [HttpPost("sendText")]
        public async Task<IActionResult> SendText(MReq_SendText request)
        {
            var res = await _s_ChatRoom.SendText(request);
            return Ok(res);
        }

        [HttpPost("sendImage")] 
        public async Task<IActionResult> SendImage(MReq_SendImage request)
        {
            var res = await _s_ChatRoom.SendImage(request);
            return Ok(res);
        }

        [HttpPost("sendVideo")]
        public async Task<IActionResult> SendVideo(MReq_SendVideo request)
        {
            var res = await _s_ChatRoom.SendVideo(request);
            return Ok(res);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllChatRoomForUser(Guid userId)
        {
            var res = await _s_ChatRoom.GetAllChatRoom(userId);
            return Ok(res);
        }

        [HttpGet("chatRoom/{id}")]
        public async Task<IActionResult> GetChatRoomById(int id)
        {
            var res = await _s_ChatRoom.GetCertainChat(id);
            return Ok(res);
        }
    }
}
