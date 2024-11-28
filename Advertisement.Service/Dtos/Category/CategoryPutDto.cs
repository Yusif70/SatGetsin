
using Microsoft.AspNetCore.Http;

namespace Advertisement.Service.Dtos
{
    public class CategoryPutDto
    {
        public string Name { get; set; }
        public IFormFile? File { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
