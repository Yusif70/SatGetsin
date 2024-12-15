using AutoMapper;
using SatGetsin2.Core.Entities;
using SatGetsin2.Service.Dtos.Category;

namespace SatGetsin2.Service.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryPostDto, Category>().ReverseMap();
            CreateMap<Category, CategoryGetDto>().ReverseMap();
            CreateMap<Category, ChildCategoryDto>().ReverseMap();
        }
    }
}
