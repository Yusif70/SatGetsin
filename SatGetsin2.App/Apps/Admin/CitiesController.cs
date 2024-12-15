using Microsoft.AspNetCore.Mvc;
using SatGetsin2.Service.Dtos.City;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.App.Apps.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, SuperAdmin")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }
        [HttpPost("create")]
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
