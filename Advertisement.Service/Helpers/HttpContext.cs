using Advertisement.Service.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Advertisement.Service.Helpers
{
    public class HttpContext
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string GetUserId()
        {
            var claim = _contextAccessor.HttpContext.User.Identities.First().Claims.FirstOrDefault(c => c.Type == "Id");
            return claim == null ? throw new TokenInvalidException() : claim.Value;
        }
    }
}
