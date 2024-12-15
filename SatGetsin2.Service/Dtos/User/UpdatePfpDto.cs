using Microsoft.AspNetCore.Http;

namespace SatGetsin2.Service.Dtos.User
{
    public class UpdatePfpDto
    {
        public string? UserId { get; set; }
        public IFormFile? File { get; set; }
    }
}
