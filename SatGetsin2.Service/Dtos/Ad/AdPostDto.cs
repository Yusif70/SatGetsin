using Microsoft.AspNetCore.Http;
using SatGetsin2.Core.Enums;

namespace SatGetsin2.Service.Dtos.Ad
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
        // TODO : optional price
        // public bool ByAgreement
        public double Price { get; set; }
        public string Description { get; set; }
        public IFormFile Video { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
