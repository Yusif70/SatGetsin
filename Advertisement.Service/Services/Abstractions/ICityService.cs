using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.City;

namespace Advertisement.Service.Services.Abstractions
{
    public interface ICityService
    {
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetById(Guid id);
        Task<ApiResponse> Create(CityPostDto dto);
        Task<ApiResponse> Update(Guid id, CityPutDto dto);
        Task<ApiResponse> Delete(Guid id);
    }
}
