using SatGetsin2.Service.Exceptions;
using System.Security.Claims;

namespace SatGetsin2.Service.Extensions
{
    public static class ClaimExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (user.Claims == null)
                throw new ArgumentNullException(nameof(user.Claims));

            var claim = user.Identities.First().Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim == null ? throw new InvalidTokenException() : claim.Value;
        }
    }
}
