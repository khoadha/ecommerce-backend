using DataAccessLayer.BusinessModels;
using DataAccessLayer.Models;

namespace EXE_API.Services.ApplicationUserService
{
    public interface IApplicationUserService {

        Task<ServiceResponse<List<ApplicationUser>>> GetUsers();
        Task<ServiceResponse<ApplicationUser>> GetUserByEmail(string email);
        Task<ServiceResponse<ApplicationUser>> GetUserById(string id);
        Task<ServiceResponse<bool>> IsUserExist(string id);
        Task<ServiceResponse<int>> GetWemadePoint(string id);
        bool Save();
    }
}
