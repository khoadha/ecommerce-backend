using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;

namespace EXE101_API.Services.ChatService
{
    public interface IChatService
    {
        Task<ServiceResponse<ChatMessage>> AddChat(ChatMessage message);

        Task<ServiceResponse<List<ChatMessage>>> GetChat(string connectionId);
    }
}
