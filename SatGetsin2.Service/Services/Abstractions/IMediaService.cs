using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace SatGetsin2.Service.Services.Abstractions
{
    public interface IMediaService
    {
        Task<ImageUploadResult> ImageUploadAsync(IFormFile file);
        Task<VideoUploadResult> VideoUploadAsync(IFormFile file);
        Task<DeletionResult> DeleteMediaAsync(string id);
        Task<RestoreResult> RestoreMediaAsync(List<string> ids);
    }
}
