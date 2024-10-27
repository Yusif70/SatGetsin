using Advertisement.Service.Dtos.Auth;
using Advertisement.Service.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.App.Apps.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            var res = await _authService.Register(dto);
            return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            var res = await _authService.Login(dto);
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(IdentityUser user, string token, int input)
        {
            var res = await _authService.VerifyEmail(user, token, input);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
