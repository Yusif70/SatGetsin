using Advertisement.Service.Dtos.Category;
using Advertisement.Service.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.App.Apps.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, SuperAdmin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryPostDto dto)
        {
            var res = await _categoryService.Create(dto);
            return StatusCode(res.StatusCode, res.Message);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CategoryPutDto dto)
        {
            var res = await _categoryService.Update(id, dto);
            return StatusCode(res.StatusCode);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var res = await _categoryService.Delete(id);
            return StatusCode(res.StatusCode);
        }
    }
}
