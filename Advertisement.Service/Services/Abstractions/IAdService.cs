using Advertisement.Core.Enums;
using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.Ad;

namespace Advertisement.Service.Services.Abstractions
{
    public interface IAdService
    {
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetAdsByLevel(AdLevel level);
        Task<ApiResponse> GetLatest();
        Task<ApiResponse> GetById(Guid id);
        Task<ApiResponse> GetMyAds(string userId);
        Task<ApiResponse> Create(AdPostDto dto);
        Task<ApiResponse> Update(Guid id, AdPutDto dto);
        Task<ApiResponse> Delete(Guid id);
        Task<ApiResponse> DeleteRange(List<Guid> adIds);
        Task<ApiResponse> Promote(PromoteDto dto);
        Task<ApiResponse> AddToFavorites(FavoritePostDto dto);
        Task<ApiResponse> GetFavorites(string userId);
        Task<ApiResponse> RemoveFromFavorites(Guid id);
    }
}
