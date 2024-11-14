using Advertisement.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Advertisement.Service.Dtos.Ad
{
    public class AdPostDto
    {
        public string? UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Whatsapp { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CityId { get; set; }
        public ProductState State { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string VideoLink { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
