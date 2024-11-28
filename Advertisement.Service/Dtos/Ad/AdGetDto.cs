using Advertisement.Core.Enums;
using Advertisement.Service.Dtos.City;
using Advertisement.Service.Dtos.Image;

namespace Advertisement.Service.Dtos.Ad
{
    public class AdGetDto
    {
        public UserDto User { get; set; }
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Whatsapp { get; set; }
        public string Name { get; set; }
        public ChildCategoryDto Category { get; set; }
        public CityGetDto City { get; set; }
        public ProductState State { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string VideoLink { get; set; }
        public List<ImageGetDto> Images { get; set; }
        public AdLevel Level { get; set; }
    }
    public class UserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}