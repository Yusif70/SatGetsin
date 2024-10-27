using Advertisement.Service.Dtos.Ad;
using Advertisement.Service.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.App.Apps.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public class AdController : ControllerBase
    {
        private readonly IAdService _adService;

        public AdController(IAdService adService)
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
            var res = await _adService.GetVip();
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
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AdPostDto dto)
        {
            var res = await _adService.Create(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(Guid id, AdPutDto dto)
        //{
        //    var res = await _adService.Update(id, dto);
        //    return StatusCode(res.StatusCode);
        //}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _adService.Delete(id);
            return StatusCode(res.StatusCode);
        }
    }
}
