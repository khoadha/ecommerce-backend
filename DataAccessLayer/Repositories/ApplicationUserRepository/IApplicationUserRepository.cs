using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories {
    public interface IApplicationUserRepository {
        Task<List<ApplicationUser>> GetUsers();
        Task<ApplicationUser> GetUserByEmail(string email);
        Task<ApplicationUser> GetUserById(string id);
        Task<bool> IsUserExist(string id);
        Task<int> GetWemadePoint(string id);
        bool Save();
    }
}
