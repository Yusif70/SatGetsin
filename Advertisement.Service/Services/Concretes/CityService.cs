using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.City;
using Advertisement.Service.Services.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Advertisement.Service.Services.Concretes
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CityService(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse> GetAll()
        {
            var dtos = await _cityRepository.GetAll(c => !c.IsDeleted).Select(c => new CityGetDto { Id = c.Id, Name = c.Name }).ToListAsync();
            return new ApiResponse { StatusCode = 200, Data = dtos };
        }

        public async Task<ApiResponse> GetById(Guid id)
        {
            var city = await _cityRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (city == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "City not found" };
            }
            var dto = _mapper.Map<CityGetDto>(city);
            return new ApiResponse { StatusCode = 200, Data = dto };
        }
        public async Task<ApiResponse> Create(CityPostDto dto)
        {
            var city = _mapper.Map<City>(dto);
            city.CreatedAt = DateTime.Now;
            await _cityRepository.AddAsync(city);
            await _cityRepository.SaveAsync();
            return new ApiResponse { StatusCode = 201, Message = "City created successfully!" };
        }

        public async Task<ApiResponse> Delete(Guid id)
        {
            var city = await _cityRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (city == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "City not found" };
            }
            city.IsDeleted = true;
            await _cityRepository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }

        //public async Task<ApiResponse> Update(Guid id, CityPutDto dto)
        //{
        //    var city = await _cityRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
        //    if (city == null)
        //    {
        //        return new ApiResponse { StatusCode = 404, Message = "City not found" };
        //    }
        //    city.Name = dto.Name;
        //    city.LastUpdatedAt = DateTime.Now;
        //    _cityRepository.Update(city);
        //    await _cityRepository.SaveAsync();
        //    return new ApiResponse { StatusCode = 204 };
        //}
    }
}
