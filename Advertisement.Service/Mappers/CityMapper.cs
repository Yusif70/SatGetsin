using Advertisement.Core.Entities;
using Advertisement.Service.Dtos.City;
using AutoMapper;

namespace Advertisement.Service.Mappers
{
    public class CityMapper : Profile
    {
        public CityMapper()
        {
            CreateMap<CityPostDto, City>().ReverseMap();
            CreateMap<City, CityGetDto>().ReverseMap();
        }
    }
}
