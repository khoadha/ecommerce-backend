using DataAccessLayer.Models;
using DataAccessLayer.Repositories.ChatRepository;
using DataAccessLayer.Shared;
using EXE101_API.Context;
using EXE101_API.Services.ChatService;
using Microsoft.AspNetCore.SignalR;

namespace EXE101_API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chatRepository;

        private readonly IChatService _chatService;
        private readonly IUserContext _userContext;
        public ChatHub(IChatRepository chatRepository, IChatService chatService, IUserContext userContext)
        {
            _chatRepository = chatRepository;
            _chatService = chatService;
            _userContext = userContext;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "WeMadeChat");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "WeMadeChat");
            var user = _chatRepository.GetUserByConnectionId(Context.ConnectionId);
            _chatRepository.RemoveUserFromList(user);

            //await DisplayOnlineUser();            

            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string name)
        {
            _chatRepository.AddUserConnectionId(name, Context.ConnectionId);
            await DisplayOnlineUser(name);
        }

        public async Task DisplayOnlineUser(string name)
        {
            var onlineUsers = await _chatRepository.GetOnlineUsers(name);
            await Clients.Groups("WeMadeChat").SendAsync("OnlineUsers", onlineUsers);
        }


        public async Task ReceiveMessage(MessageDto message)
        {   
            await Clients.Group("WeMadeChat").SendAsync("NewMessage", message);
        }

        public async Task CreatePrivateChat(MessageDto message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatRepository.GetConnectionIdByUser(message.To);
            if(toConnectionId != null)
            {
                await Groups.AddToGroupAsync(toConnectionId, privateGroupName);
                // add 2nd user to group chat
                await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
            }
           
            await GetPrivateChat(privateGroupName);


        }

        public async Task GetPrivateChat(string groupName)
        {
            var serviceResponse = await _chatService.GetChat(groupName);

            if (serviceResponse.Success)
            {
                List<MessageDto> messageDtos = serviceResponse.Data.Select(chatMessage => new MessageDto
                {
                    From = chatMessage.FromUser,
                    To = chatMessage.ToUser,
                    Content = chatMessage.MessageText,
                    Time = chatMessage.SentDateTime
                }).ToList();

                await Clients.Group(groupName).SendAsync("GetPrivateChatList", messageDtos);
            }
            else
            {
                await Clients.Group(groupName).SendAsync("GetPrivateChatList", new List<MessageDto>());
            }
        }
        
        public async Task ReceivePrivateMessage(MessageDto message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
            ChatMessage chat = new ChatMessage
            {
                MessageText = message.Content,
                ConnectionId = privateGroupName,
                FromUser = message.From,
                ToUser = message.To,
                SentDateTime = DateTime.Now,
                NewProperty = "Some additional information"
            };
            await _chatService.AddChat(chat);
        }

        public async Task RemovePrivateMessage(MessageDto message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Clients.Group(privateGroupName).SendAsync("ClosePrivateMessage", message);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatRepository.GetConnectionIdByUser(message.To);
            if (toConnectionId != null)
            {
                await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
            }
            
        }

        public string GetPrivateGroupName(string from, string to)
        {
            var stringCompare = string.CompareOrdinal(from, to) < 0;
            return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
        }
    }
}
