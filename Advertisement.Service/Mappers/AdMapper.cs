using Advertisement.Core.Entities;
using Advertisement.Service.Dtos.Ad;
using AutoMapper;

namespace Advertisement.Service.Mappers
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
