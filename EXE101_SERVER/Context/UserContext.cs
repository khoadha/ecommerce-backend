using DataAccessLayer.Shared;
using System.IdentityModel.Tokens.Jwt;

namespace EXE101_API.Context {
    public class UserContext : IUserContext {

        public UserContext() {
        }

        public CurrentUser GetCurrentUser(HttpContext context) {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader != null && authorizationHeader.StartsWith("bearer ")) {
                var token = authorizationHeader.Substring("bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(token);

                var result = new CurrentUser() {
                    UserId = decodedToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value,
                    UserName = decodedToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                    Email = decodedToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                    Role = (decodedToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value),
                };

                var managedStoreId = decodedToken.Claims.FirstOrDefault(c => c.Type == "ManagedStoreId")?.Value;

                if (managedStoreId != null) {
                    result.ManagedStoreId = int.Parse(managedStoreId);
                }


                return result;
            }
            return null;
        }
    }
}
