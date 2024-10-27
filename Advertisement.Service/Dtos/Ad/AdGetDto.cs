using Advertisement.Core.Enums;

namespace Advertisement.Service.Dtos.Ad
{
    public class AdGetDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Whatsapp { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Core.Entities.Category Category { get; set; }
        public Guid SubCategoryId { get; set; }
        public Core.Entities.Category SubCategory { get; set; }
        public Guid CityId { get; set; }
        public Core.Entities.City City { get; set; }
        public ProductState State { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public string VideoLink { get; set; }
        public List<Core.Entities.Image> Images { get; set; }
        public AdLevel Level { get; set; }
    }
}
