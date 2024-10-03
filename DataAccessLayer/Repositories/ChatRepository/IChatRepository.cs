using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.ChatRepository
{
    public interface IChatRepository
    {
        public bool AddUserToList(string userToAdd);
        public void AddUserConnectionId(string user, string connectionId);
        public string GetUserByConnectionId(string connectionId);
        public string GetConnectionIdByUser(string user);
        public void RemoveUserFromList(string user);
        public  Task<List<UserList>> GetOnlineUsers(string email);

        public Task<ChatMessage> AddMessage(ChatMessage message);
        public Task<List<ChatMessage>> GetMessagesByConnectionId(string connectionId);
        public Task<List<String>> GetUsersGroupHistory(string email);
    }
}
