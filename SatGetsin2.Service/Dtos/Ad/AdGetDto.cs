using SatGetsin2.Core.Enums;
using SatGetsin2.Service.Dtos.Category;
using SatGetsin2.Service.Dtos.City;
using SatGetsin2.Service.Dtos.Image;
using SatGetsin2.Service.Dtos.User;

namespace SatGetsin2.Service.Dtos.Ad
{
	public class AdGetDto
	{
		public Guid Id { get; set; }
		public UserDto User { get; set; }
		public string Name { get; set; }
		public ChildCategoryDto Category { get; set; }
		public CityGetDto City { get; set; }
		public ProductState State { get; set; }
		public ProductStatus Status { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public string VideoLink { get; set; }
		public List<ImageGetDto> Images { get; set; }
		public AdLevel Level { get; set; }
		public DateTime ExpireDate { get; set; }
	}

}
