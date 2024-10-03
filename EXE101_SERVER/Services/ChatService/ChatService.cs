
using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.ChatRepository;
using Microsoft.EntityFrameworkCore;

namespace EXE101_API.Services.ChatService
{
    public class ChatService : IChatService
{
        private readonly IChatRepository _repo;

        public ChatService(IChatRepository repo)
        {
            _repo = repo;
        }

        public async Task<ServiceResponse<ChatMessage>> AddChat(ChatMessage message)
        {
            var serviceResponse = new ServiceResponse<ChatMessage>();
            try
            {
                var response = await _repo.AddMessage(message);
                serviceResponse.Data = response;
            }
            catch (DbUpdateException ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ChatMessage>>> GetChat(string connectionId)
        {
            var serviceResponse = new ServiceResponse<List<ChatMessage>>();
            try
            {
                var response = await _repo.GetMessagesByConnectionId(connectionId);
                serviceResponse.Data = response;
            }
            catch (DbUpdateException ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
