using Advertisement.Service.Dtos.Ad;
using Advertisement.Service.Extensions;
using Advertisement.Service.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.App.Apps.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IAdService _adService;

        public AdsController(IAdService adService)
        {
            _adService = adService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _adService.GetAll();
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpGet("vip")]
        public async Task<IActionResult> GetVip()
        {
            var res = await _adService.GetVip();
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpGet("premium")]
        public async Task<IActionResult> GetPremium()
        {
            var res = await _adService.GetPremium();
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var res = await _adService.GetLatest();
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _adService.GetById(id);
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> Create([FromForm] AdPostDto dto)
        {
            dto.UserId = User.GetUserId();
            var res = await _adService.Create(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> Update(Guid id, [FromForm] AdPutDto dto)
        {
            var res = await _adService.Update(id, dto);
            return StatusCode(res.StatusCode);
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _adService.Delete(id);
            return StatusCode(res.StatusCode);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("promote/{id}")]
        public async Task<IActionResult> Promote(PromoteDto dto)
        {
            dto.UserId = User.GetUserId();
            var res = await _adService.Promote(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("addToFavorites/{id}")]
        public async Task<IActionResult> AddToFavorites(FavoriteDto dto)
        {
            dto.UserId = User.GetUserId();
            var res = await _adService.AddToFavorites(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("removeFromFavorites/{id}")]
        public async Task<IActionResult> RemoveFromFavorites(Guid id)
        {
            var res = await _adService.RemoveFromFavorites(id);
            return StatusCode(res.StatusCode);
        }
    }
}
