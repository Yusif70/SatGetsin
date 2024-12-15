using Microsoft.AspNetCore.Mvc;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.App.Apps.User
{
    [Route("api/[controller]")]
    [ApiController]

    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _cityService.GetAll();
            return StatusCode(res.StatusCode, res.Data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _cityService.GetById(id);
            return StatusCode(res.StatusCode, res.Data);
        }
    }
}
