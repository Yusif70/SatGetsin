using Advertisement.Service.Dtos.User;
using Advertisement.Service.Extensions;
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
        [HttpPost("chargeCard")]
        public async Task<IActionResult> ChargeCard()
        {
            var res = await _userService.ChargeCard();
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpPost("topUpBalance")]
        public async Task<IActionResult> TopUpBalance(PaymentDto dto)
        {
            dto.UserId = User.GetUserId();
            var res = await _userService.TopUpBalance(dto.PaymentMethodId, dto.UserId, dto.Amount);
            return StatusCode(res.StatusCode, res.Message);
        }
        [HttpPut("updateUserPfp")]
        public async Task<IActionResult> UpdateUserPfp([FromForm] UpdatePfpDto dto)
        {
            dto.UserId = User.GetUserId();
            var res = await _userService.UpdatePfp(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
