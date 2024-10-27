using Advertisement.Core.Entities;
using Advertisement.Service.Dtos.Category;
using AutoMapper;

namespace Advertisement.Service.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryPostDto, Category>().ReverseMap();
            CreateMap<Category, CategoryGetDto>().ReverseMap();
        }
    }
}
