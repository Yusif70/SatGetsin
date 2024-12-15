using AutoMapper;
using SatGetsin2.Core.Entities;
using SatGetsin2.Service.Dtos.Ad;

namespace SatGetsin2.Service.Mappers
{
    public class AdMapper : Profile
    {
        public AdMapper()
        {
            CreateMap<AdPostDto, Ad>().ReverseMap();
            CreateMap<Ad, AdGetDto>().ReverseMap();
        }
    }
}
