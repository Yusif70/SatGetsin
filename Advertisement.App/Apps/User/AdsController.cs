using Advertisement.Core.Enums;
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
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _adService.GetAll();
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpGet("getByLevel")]
        public async Task<IActionResult> GetAdsByLevel(AdLevel level)
        {
            var res = await _adService.GetAdsByLevel(level);
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpGet("getLatest")]
        public async Task<IActionResult> GetLatest()
        {
            var res = await _adService.GetLatest();
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _adService.GetById(id);
            return StatusCode(res.StatusCode, res.Data);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("getMyAds")]
        public async Task<IActionResult> GetMyAds()
        {
            string userId = User.GetUserId();
            var res = await _adService.GetMyAds(userId);
            return StatusCode(res.StatusCode, res.Data);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] AdPostDto dto)
        {
            dto.UserId = User.GetUserId();
            var res = await _adService.Create(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] AdPutDto dto)
        {
            var res = await _adService.Update(id, dto);
            return StatusCode(res.StatusCode);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpDelete("delete/{id}")]
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
        public async Task<IActionResult> AddToFavorites(FavoritePostDto dto)
        {
            dto.UserId = User.GetUserId();
            var res = await _adService.AddToFavorites(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var userId = User.GetUserId();
            var res = await _adService.GetFavorites(userId);
            return StatusCode(res.StatusCode, res.Data);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("removeFromFavorites/{id}")]
        public async Task<IActionResult> RemoveFromFavorites(Guid id)
        {
            var res = await _adService.RemoveFromFavorites(id);
            return StatusCode(res.StatusCode);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpDelete("deleteRange")]
        public async Task<IActionResult> DeleteRange(List<Guid> adIds)
        {
            var res = await _adService.DeleteRange(adIds);
            return StatusCode(res.StatusCode);
        }
    }
}
