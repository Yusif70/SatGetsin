using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SatGetsin2.Core.Entities;
using SatGetsin2.Core.Repositories.Abstractions;
using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.City;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.Service.Services.Concretes
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
            var entities = await _cityRepository.GetAll(c => !c.IsDeleted).ToListAsync();
            var dtos = entities.Select(c => new CityGetDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
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

        public async Task<ApiResponse> Update(Guid id, CityPutDto dto)
        {
            var city = await _cityRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (city == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "City not found" };
            }
            city.Name = dto.Name;
            city.LastUpdatedAt = DateTime.Now;
            _cityRepository.Update(city);
            await _cityRepository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }
    }
}
