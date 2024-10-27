using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.Category;

namespace Advertisement.Service.Services.Abstractions
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
