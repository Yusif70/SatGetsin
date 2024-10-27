using Advertisement.Service.Dtos.City;
using Advertisement.Service.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.App.Apps.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, SuperAdmin")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CityPostDto dto)
        {
            var res = await _cityService.Create(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CityPutDto dto)
        {
            var res = await _cityService.Update(id, dto);
            return StatusCode(res.StatusCode);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _cityService.Delete(id);
            return StatusCode(res.StatusCode);
        }
    }
}
