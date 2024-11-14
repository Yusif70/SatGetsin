using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.Ad;

namespace Advertisement.Service.Services.Abstractions
{
    public interface IAdService
    {
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetVip();
        Task<ApiResponse> GetPremium();
        Task<ApiResponse> GetLatest();
        Task<ApiResponse> GetById(Guid id);
        Task<ApiResponse> Create(AdPostDto dto);
        Task<ApiResponse> Update(Guid id, AdPutDto dto);
        Task<ApiResponse> Delete(Guid id);
        Task<ApiResponse> Promote(PromoteDto dto);
        Task<ApiResponse> AddToFavorites(FavoriteDto dto);
        Task<ApiResponse> RemoveFromFavorites(Guid id);
    }
}
