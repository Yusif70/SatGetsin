using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Advertisement.Service.Services.Abstractions
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string id);
    }
}
