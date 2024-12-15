using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.Category;

namespace SatGetsin2.Service.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetById(Guid id);
        Task<ApiResponse> Create(CategoryPostDto dto);
        Task<ApiResponse> Update(Guid id, CategoryPutDto dto);
        Task<ApiResponse> Delete(Guid id);
    }
}
