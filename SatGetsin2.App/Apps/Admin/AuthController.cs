using Microsoft.AspNetCore.Mvc;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.App.Apps.Admin
{
    [Route("api/[controller]")]
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
