using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SatGetsin2.Service.Helpers;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.Service.Services.Concretes
{
	public class MediaService : IMediaService
	{
		private readonly Cloudinary _cloudinary;

		public MediaService(IOptions<CloudinarySettings> config)
		{
			Account acc = new()
			{
				//Cloud = config.Value.CloudName,
				//ApiKey = config.Value.ApiKey,
				//ApiSecret = config.Value.ApiSecret
				Cloud = "deylhgaan",
				ApiKey = "149381938114127",
				ApiSecret = "6Gp5Ie79NYT-moGknMCQuSSF-j0"
			};
			_cloudinary = new Cloudinary(acc);
		}

		public async Task<ImageUploadResult> ImageUploadAsync(IFormFile file)
		{
			var uploadResult = new ImageUploadResult();
			if (file.Length > 0)
			{
				using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					PublicId = Guid.NewGuid().ToString() + Path.GetFileNameWithoutExtension(file.FileName).Trim(),
					Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
				};
				uploadResult = await _cloudinary.UploadAsync(uploadParams);
			}
			return uploadResult;
		}
		public async Task<VideoUploadResult> VideoUploadAsync(IFormFile file)
		{
			var uploadResult = new VideoUploadResult();
			if (file.Length > 0)
			{
				using var stream = file.OpenReadStream();
				var uploadParams = new VideoUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					PublicId = Guid.NewGuid().ToString() + Path.GetFileNameWithoutExtension(file.FileName).Trim(),
					EagerTransforms = new List<Transformation>()
					{
						new EagerTransformation().Width(300).Height(300).Crop("pad").AudioCodec("none"),
						new EagerTransformation().Width(160).Height(100).Crop("crop").Gravity("south").AudioCodec("none"),
					},
					EagerAsync = true,
				};
				uploadResult = await _cloudinary.UploadAsync(uploadParams);
			}
			return uploadResult;
		}
		public async Task<DeletionResult> DeleteMediaAsync(string id)
		{
			var deleteParams = new DeletionParams(id);
			var result = await _cloudinary.DestroyAsync(deleteParams);
			return result;
		}
		public async Task<RestoreResult> RestoreMediaAsync(List<string> ids)
		{
			var restoreParams = new RestoreParams()
			{
				PublicIds = ids,
			};
			var result = await _cloudinary.RestoreAsync(restoreParams);
			return result;
		}
	}
}
