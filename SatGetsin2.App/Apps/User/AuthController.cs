using Microsoft.AspNetCore.Mvc;
using SatGetsin2.Service.Dtos.Auth;
using SatGetsin2.Service.Extensions;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.App.Apps.User
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            var res = await _authService.Register(dto);
            return StatusCode(res.StatusCode, res.Data);
        }

        [HttpPost("verifyEmail")]
        public async Task<IActionResult> VerifyEmail(string userId, string token, int input)
        {
            var res = await _authService.VerifyEmail(userId, token, input);
            return StatusCode(res.StatusCode, res.Message);
        }
        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendConfirmationEmail()
        {
            var userId = User.GetUserId();
            var res = await _authService.SendConfirmationEmail(userId);
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpPost("verifyPhone")]
        public async Task<IActionResult> VerifyPhone(string userId, string token, int input)
        {
            var res = await _authService.VerifyPhone(userId, token, input);
            return StatusCode(res.StatusCode, res.Message);
        }
        [HttpPost("sendSms")]
        public async Task<IActionResult> SendConfirmationSms(string userId)
        {
            var res = await _authService.SendConfirmationSms(userId);
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            var res = await _authService.Login(dto);
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpPost("logout")]
        public void Logout()
        {
            _authService.Logout();
        }
    }
}
