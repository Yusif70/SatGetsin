using Advertisement.Service.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.App.Apps.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, SuperAdmin")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("createRoles")]
        public async Task<IActionResult> CreateRoles()
        {
            var res = await _authService.CreateRole();
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
