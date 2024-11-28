using Microsoft.AspNetCore.Http;

namespace Advertisement.Service.Dtos.User
{
    public class UpdatePfpDto
    {
        public string? UserId { get; set; }
        public IFormFile File { get; set; }
    }
}
