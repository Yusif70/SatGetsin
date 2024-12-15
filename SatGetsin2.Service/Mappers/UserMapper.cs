using AutoMapper;
using SatGetsin2.Core.Entities;
using SatGetsin2.Service.Dtos.Ad;

namespace SatGetsin2.Service.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
