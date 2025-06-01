using ChatService.Common;
using ChatService.Models.Dtos.RequestModel;
using ChatService.Models.Dtos.ResponseModel;
using ChatService.Models.Entities;
using ChatService.SyncDataService;
using ChatService.Ultils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ChatService.Service
{
    public interface IS_ChatRoom
    {
        Task<ResponseData<MRes_ChatMessage>> SendText(MReq_SendText request);
        Task<ResponseData<MRes_ChatMessage>> SendImage(MReq_SendImage request);
        Task<ResponseData<MRes_ChatMessage>> SendVideo(MReq_SendVideo request);
        Task<ResponseData<List<MRes_ChatRoom>>> GetAllChatRoom(Guid userId);
        Task<ResponseData<List<MRes_ChatMessage>>> GetCertainChat(int chatRoomId);
    }
    public class S_ChatRoom : IS_ChatRoom
    {
        private readonly ChatDBContext _context;
        private readonly IS_UserDataClient _s_UserDataClient;
        private readonly IHubContext<ChatHub> _hubContext;

        public S_ChatRoom(ChatDBContext context, IS_UserDataClient s_UserDataClient, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _s_UserDataClient = s_UserDataClient;
            _hubContext = hubContext;
        }

        public async Task<ResponseData<MRes_ChatMessage>> SendText(MReq_SendText request)
        {
            var res = new ResponseData<MRes_ChatMessage>();
            try
            {
                var chatRoom = await _context.ChatRooms.Where(
                p => p.FirstUserId == request.UserSendId && p.SecondUserId == request.UserReceiveId ||
                    p.FirstUserId == request.UserReceiveId && p.SecondUserId == request.UserSendId).FirstOrDefaultAsync();
                if (chatRoom == null)
                {
                    chatRoom = new ChatRoom()
                    {
                        Id = await _context.ChatRooms.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync() + 1,
                        FirstUserId = request.UserSendId,
                        SecondUserId = request.UserReceiveId,
                    };
                    _context.ChatRooms.Add(chatRoom);
                }
                var chatMessage = new ChatMessage()
                {
                    ChatRoomId = chatRoom.Id,
                    MediaLink = null,
                    Message = request.Message,
                    UserSendId = request.UserSendId,
                    SendDate = DateTime.Now,
                    Type = "Text"
                };
                _context.ChatMessages.Add(chatMessage);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    return res;
                }

                var returnData = await _context.ChatMessages.FindAsync(chatMessage.Id);
                res.result = 1;
                res.data = new MRes_ChatMessage
                {
                    Id = returnData.Id,
                    MediaLink = returnData.MediaLink,
                    Message = returnData.Message,
                    SendDate = returnData.SendDate,
                    Type = returnData.Type,
                    UserSendId = returnData.UserSendId
                };
                res.error.message = MessageErrorConstants.CREATE_SUCCESS;
                await _hubContext.Clients.All.SendAsync("NewMessage", chatRoom.SecondUserId.ToString());
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<MRes_ChatMessage>> SendImage(MReq_SendImage request)
        {
            var res = new ResponseData<MRes_ChatMessage>();
            try
            {
                var chatRoom = await _context.ChatRooms.Where(
                p => p.FirstUserId == request.UserSendId && p.SecondUserId == request.UserReceiveId ||
                    p.FirstUserId == request.UserReceiveId && p.SecondUserId == request.UserSendId).FirstOrDefaultAsync();
                if (chatRoom == null)
                {
                    chatRoom = new ChatRoom()
                    {
                        Id = await _context.ChatRooms.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync() + 1,
                        FirstUserId = request.UserSendId,
                        SecondUserId = request.UserReceiveId,
                    };
                    _context.ChatRooms.Add(chatRoom);
                }
                var chatMessage = new ChatMessage()
                {
                    ChatRoomId = chatRoom.Id,
                    MediaLink = request.MediaLink,
                    UserSendId = request.UserSendId,
                    SendDate = DateTime.Now,
                    Type = "Image"
                };
                _context.ChatMessages.Add(chatMessage);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    return res;
                }

                var returnData = await _context.ChatMessages.FindAsync(chatMessage.Id);
                res.result = 1;
                res.data = new MRes_ChatMessage
                {
                    Id = returnData.Id,
                    MediaLink = returnData.MediaLink,
                    Message = returnData.Message,
                    SendDate = returnData.SendDate,
                    Type = returnData.Type,
                    UserSendId = returnData.UserSendId
                };
                res.error.message = MessageErrorConstants.CREATE_SUCCESS;
                await _hubContext.Clients.All.SendAsync("NewMessage", chatRoom.SecondUserId.ToString());
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<MRes_ChatMessage>> SendVideo(MReq_SendVideo request)
        {
            var res = new ResponseData<MRes_ChatMessage>();
            try
            {
                var chatRoom = await _context.ChatRooms.Where(
                p => p.FirstUserId == request.UserSendId && p.SecondUserId == request.UserReceiveId ||
                    p.FirstUserId == request.UserReceiveId && p.SecondUserId == request.UserSendId).FirstOrDefaultAsync();
                if (chatRoom == null)
                {
                    chatRoom = new ChatRoom()
                    {
                        Id = await _context.ChatRooms.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync() + 1,
                        FirstUserId = request.UserSendId,
                        SecondUserId = request.UserReceiveId,
                    };
                    _context.ChatRooms.Add(chatRoom);
                }
                var chatMessage = new ChatMessage()
                {
                    ChatRoomId = chatRoom.Id,
                    MediaLink = request.MediaLink,
                    UserSendId = request.UserSendId,
                    SendDate = DateTime.Now,
                    Type = "Video"
                };
                _context.ChatMessages.Add(chatMessage);
                var save = await _context.SaveChangesAsync();
                if (save == 0)
                {
                    res.error.code = 400;
                    res.error.message = MessageErrorConstants.EXCEPTION_DO_NOT_CREATE;
                    return res;
                }

                var returnData = await _context.ChatMessages.FindAsync(chatMessage.Id);
                res.result = 1;
                res.data = new MRes_ChatMessage
                {
                    Id = returnData.Id,
                    MediaLink = returnData.MediaLink,
                    Message = returnData.Message,
                    SendDate = returnData.SendDate,
                    Type = returnData.Type,
                    UserSendId = returnData.UserSendId
                };
                res.error.message = MessageErrorConstants.CREATE_SUCCESS;
                await _hubContext.Clients.All.SendAsync("NewMessage", chatRoom.SecondUserId.ToString());
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_ChatRoom>>> GetAllChatRoom(Guid userId)
        {
            var res = new ResponseData<List<MRes_ChatRoom>>();
            try
            {
                var chatRooms = await _context.ChatRooms.Where(
                p => p.FirstUserId == userId || p.SecondUserId == userId).ToListAsync();
                var returnData = new List<MRes_ChatRoom>();
                foreach (var chatRoom in chatRooms)
                {
                    var userID = userId == chatRoom.FirstUserId ? chatRoom.SecondUserId : chatRoom.FirstUserId;
                    var friend = await _s_UserDataClient.GetUserById(userID);
                    if(friend == null)
                    {
                        res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                        return res;
                    }
                    var lastMessage = await _context.ChatMessages.Where(p => p.ChatRoomId == chatRoom.Id).OrderByDescending(p => p.SendDate).FirstOrDefaultAsync();
                    var chatRoomReadDTO = new MRes_ChatRoom()
                    {
                        Id = chatRoom.Id,
                        UserId = friend.Id,
                        Avatar = friend.ImageURL,
                        Name = friend.Name,
                    };
                    if(lastMessage != null)
                    {
                        chatRoomReadDTO.LastMessage = lastMessage.Message;
                        chatRoomReadDTO.LastMessageTime = lastMessage.SendDate;
                    }
                    returnData.Add(chatRoomReadDTO);
                }
                returnData = returnData.OrderByDescending(p => p.LastMessageTime).ToList();
                res.result = 1;
                res.data = returnData;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<List<MRes_ChatMessage>>> GetCertainChat(int chatRoomId)
        {
            var res = new ResponseData<List<MRes_ChatMessage>>();
            try
            {
                var chatMessages = await _context.ChatMessages.Where(p => p.ChatRoomId == chatRoomId).ToListAsync();
                var returnData = new List<MRes_ChatMessage>();
                var chatRoom = await _context.ChatRooms.FindAsync(chatRoomId);
                if (chatRoom == null)
                {
                    res.error.message = MessageErrorConstants.DO_NOT_FIND_DATA;
                    return res;
                }
                var firstUser = await _s_UserDataClient.GetUserById(chatRoom.FirstUserId);
                var secondUser = await _s_UserDataClient.GetUserById(chatRoom.SecondUserId);
                foreach (var chatMessage in chatMessages)
                {
                    var user = chatMessage.UserSendId == firstUser.Id ? firstUser : secondUser;
                    var chatMessageReadDTO = new MRes_ChatMessage()
                    {
                        Id = chatMessage.Id,
                        UserSendId = chatMessage.UserSendId,
                        Avatar = user.ImageURL,
                        MediaLink = chatMessage.MediaLink,
                        Message = chatMessage.Message,
                        SendDate = chatMessage.SendDate,
                        Type = chatMessage.Type,
                    };
                    returnData.Add(chatMessageReadDTO);
                }
                res.result = 1;
                res.data = returnData;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

    }
}
