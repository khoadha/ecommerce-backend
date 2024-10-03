using DataAccessLayer.Shared;

namespace EXE101_API.Context
{
    public interface IUserContext {
        CurrentUser GetCurrentUser(HttpContext context);
    }
}
