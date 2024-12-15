using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.City;

namespace SatGetsin2.Service.Services.Abstractions
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
