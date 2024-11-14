using Advertisement.Service.Dtos.User;
using Advertisement.Service.Services.Concretes;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.App.Apps.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost("topUpBalance")]
        public async Task<IActionResult> TopUpBalance(PaymentDto dto)
        {
            var res = await _userService.TopUpBalance(dto.UserId, dto.Amount);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
