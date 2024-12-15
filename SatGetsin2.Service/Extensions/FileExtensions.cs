using Microsoft.AspNetCore.Http;

namespace SatGetsin2.Service.Extensions
{
    public static class FileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }
        public static bool IsVideo(this IFormFile file)
        {
            return file.ContentType.Contains("video");
        }
        public static bool IsSizeOk(this IFormFile file, int mb)
        {
            return file.Length / 1024 / 1024 <= mb;
        }
        public static async Task<string> SaveAsync(this IFormFile file, string root, string path)
        {
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string fullPath = Path.Combine(root, path, fileName);
            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
