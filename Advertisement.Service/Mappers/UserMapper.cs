using Advertisement.Core.Entities;
using Advertisement.Service.Dtos.Ad;
using AutoMapper;

namespace Advertisement.Service.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
