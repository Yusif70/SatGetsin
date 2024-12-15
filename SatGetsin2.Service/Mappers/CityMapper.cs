using AutoMapper;
using SatGetsin2.Core.Entities;
using SatGetsin2.Service.Dtos.City;

namespace SatGetsin2.Service.Mappers
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
