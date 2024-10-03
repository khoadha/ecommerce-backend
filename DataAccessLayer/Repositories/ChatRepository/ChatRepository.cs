using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.ChatRepository
{
    public class ChatRepository : IChatRepository
    {
        private static readonly Dictionary<string,string> Users = new Dictionary<string, string>();
        private readonly EXEContext _context;
        private readonly IStoreRepository _storeRepository;
        public ChatRepository(EXEContext context, IStoreRepository storeRepository)
        {
            _context = context;
            _storeRepository = storeRepository;
        }
        public bool AddUserToList(string userToAdd)
        {
            List<string> userList = GetUsersGroupHistory(userToAdd).Result;
            if (Users.Count < 1)
            {
                foreach(var myuser in  userList)
                {
                    if(myuser != userToAdd && !string.IsNullOrWhiteSpace(myuser))
                    {
                        Users.Add(myuser, null);
                    }
                    
                }
            }

            lock (Users)
            {
                
                foreach(var user in Users)
                {
                    if(user.Key.ToLower() == userToAdd.ToLower())
                    {
                        return false; //true truoc roi tinh
                    }
                   
                }

                Users.Add(userToAdd, null);

                foreach (var userHistory in userList)
                {
                    bool userExists = false;

                    foreach (var user in Users.Keys)
                    {
                        if (user.ToLower() == userHistory.ToLower())
                        {
                            userExists = true;
                            break;
                        }
                    }

                    if (!userExists)
                    {
                        Users.Add(userHistory, null);
                    }
                }
                return true;
            }
        }

        public void AddUserConnectionId(string user, string connectionId)
        {
            lock(Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users[user] = connectionId; 
                }
            }
        }

        public string GetUserByConnectionId(string connectionId) 
        {
            lock(Users)
            {
                return Users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
            }
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (Users)
            {
                return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }

        public void RemoveUserFromList(string user)
        {
            lock(Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users.Remove(user);
                }
            }
        }

        public async Task<List<UserList>> GetOnlineUsers(string email)
        {
            try
            {
                List<string> emails;
                List<string> userList = GetUsersGroupHistory(email).Result;
                lock (Users)
                {
                    emails = Users.OrderBy(x => x.Key).Select(x => x.Key).ToList();
                }
                var userDetails = await _context.Users
                                       .Where(user => emails.Contains(user.Email) && userList.Contains(user.Email))
                                       .Select(user => new UserList
                                       {
                                           Email = user.Email,
                                           Username = user.UserName,
                                           ImgPath = user.ImgPath,
                                           LastMessage = _context.ChatMessages
                                                    .Where(m => (m.FromUser == email && m.ToUser == user.Email) || (m.FromUser == user.Email && m.ToUser == email))
                                                    .OrderByDescending(m => m.SentDateTime)
                                                    .Select(m => m.MessageText)
                                                    .FirstOrDefault(),
                                           CurrentUser = email
                                       })
                                       .ToListAsync();
                return userDetails;

            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
        public async Task<List<ChatMessage>> GetMessagesByConnectionId(string connectionId)
        {
            try
            {
                var latestMessages = await _context.ChatMessages
                    .Where(m => m.ConnectionId == connectionId)
                    .OrderByDescending(m => m.SentDateTime)
                    .Take(10)
                    .ToListAsync();

                latestMessages.Reverse(); 

                return latestMessages;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ChatMessage> AddMessage(ChatMessage message)
        {
            try
            {
                message.SentDateTime = DateTime.Now;
                
                await _context.ChatMessages.AddAsync(message);
                
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return message;
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<String>> GetUsersGroupHistory(string email)
        {
            try
            {
                List<string> fromUserList =  await _context.ChatMessages
                    .Where(m => m.FromUser == email)
                    .Select(m => m.ToUser)
                    .Distinct()
                    .ToListAsync();

                List<string> toUserList = await _context.ChatMessages
                   .Where(m => m.ToUser == email)
                   .Select(m => m.FromUser)
                   .Distinct()
                   .ToListAsync();

                return fromUserList.Union(toUserList).Distinct().ToList(); 
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
