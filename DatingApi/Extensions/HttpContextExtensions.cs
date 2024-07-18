using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DatingApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static string getCurrentUserId (this HttpContext http)
        {
            return http.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
