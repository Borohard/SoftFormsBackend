using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SoftForms.API.Services
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
