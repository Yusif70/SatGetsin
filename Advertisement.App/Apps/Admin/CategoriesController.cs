using Advertisement.Service.Dtos;
using Advertisement.Service.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.App.Apps.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, SuperAdmin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CategoryPostDto dto)
        {
            var res = await _categoryService.Create(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CategoryPutDto dto)
        {
            var res = await _categoryService.Update(id, dto);
            return StatusCode(res.StatusCode);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _categoryService.Delete(id);
            return StatusCode(res.StatusCode);
        }
    }
}
