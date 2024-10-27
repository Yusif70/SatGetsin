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
        private readonly ICityRepository _repository;
        private readonly IMapper _mapper;

        public CityService(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ApiResponse> GetAll()
        {
            var entities = await _repository.GetAll(c => !c.IsDeleted).ToListAsync();
            return new ApiResponse { StatusCode = 200, Data = entities };
        }

        public async Task<ApiResponse> GetById(Guid id)
        {
            var entity = await _repository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (entity == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "City not found" };
            }
            var dto = _mapper.Map<CityGetDto>(entity);
            return new ApiResponse { StatusCode = 200, Data = dto };
        }
        public async Task<ApiResponse> Create(CityPostDto dto)
        {
            var entity = _mapper.Map<City>(dto);
            entity.CreatedAt = DateTime.Now;
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
            return new ApiResponse { StatusCode = 201, Message = "City created successfully!" };
        }

        public async Task<ApiResponse> Delete(Guid id)
        {
            var entity = await _repository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (entity == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "City not found" };
            }
            entity.IsDeleted = true;
            await _repository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }

        public async Task<ApiResponse> Update(Guid id, CityPutDto dto)
        {
            var entity = await _repository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (entity == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "City not found" };
            }
            entity.Name = dto.Name;
            entity.LastUpdatedAt = DateTime.Now;
            _repository.Update(entity);
            await _repository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }
    }
}
