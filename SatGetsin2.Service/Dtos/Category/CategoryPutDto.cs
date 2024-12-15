using Microsoft.AspNetCore.Http;

namespace SatGetsin2.Service.Dtos.Category
{
    public class CategoryPutDto
    {
        public string? Name { get; set; }
        public IFormFile? File { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
