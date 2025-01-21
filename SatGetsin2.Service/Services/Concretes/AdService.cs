using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SatGetsin2.Core.Entities;
using SatGetsin2.Core.Enums;
using SatGetsin2.Core.Repositories.Abstractions;
using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.Ad;
using SatGetsin2.Service.Dtos.Category;
using SatGetsin2.Service.Dtos.City;
using SatGetsin2.Service.Dtos.Image;
using SatGetsin2.Service.Dtos.User;
using SatGetsin2.Service.Extensions;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.Service.Services.Concretes
{
	public class AdService : IAdService
	{
		private readonly IMapper _mapper;
		private readonly IAdRepository _adRepository;
		private readonly IImageRepository _imageRepository;
		private readonly IFavoriteRepository _favoriteRepository;
		private readonly IMediaService _mediaService;
		private readonly IStripeService _stripeService;
		private readonly UserManager<AppUser> _userManager;
		public AdService(IMapper mapper, IAdRepository adRepository, IImageRepository imageRepository, IMediaService mediaService, UserManager<AppUser> userManager, IFavoriteRepository favoriteRepository, IStripeService stripeService)
		{
			_mapper = mapper;
			_adRepository = adRepository;
			_imageRepository = imageRepository;
			_mediaService = mediaService;
			_userManager = userManager;
			_favoriteRepository = favoriteRepository;
			_stripeService = stripeService;
		}

		private IQueryable<Ad> GetAdsQuery()
		{
			return _adRepository.GetAll(a => !a.IsDeleted && a.Status == ProductStatus.Approved)
				.Include(a => a.Category)
				.Include(a => a.City)
				.Include(a => a.Images)
				.Include(a => a.User)
				.Include(a => a.FavoritedByUsers);
		}
		public async Task<ApiResponse> Create(AdPostDto dto)
		{
			var user = await _userManager.FindByIdAsync(dto.UserId);
			if (user == null)
			{
				return new ApiResponse { StatusCode = 404 };
			}
			var ad = _mapper.Map<Ad>(dto);
			ad.Level = AdLevel.Basic;
			ad.CreatedAt = DateTime.Now;
			ad.ExpireDate = DateTime.Now.AddDays(30);
			var videoUploadresult = await _mediaService.VideoUploadAsync(dto.Video);
			if (videoUploadresult.Error != null)
			{
				return new ApiResponse { StatusCode = 500, Message = videoUploadresult.Error.Message };
			}
			ad.VideoUrl = videoUploadresult.SecureUrl.AbsoluteUri;
			ad.Status = ProductStatus.Pending;
			await _adRepository.AddAsync(ad);
			var images = new List<Image>();
			for (int i = 0; i < dto.Files.Count; i++)
			{
				var file = dto.Files[i];
				var res = await _mediaService.ImageUploadAsync(file);
				if (res.Error != null)
				{
					return new ApiResponse { StatusCode = 500, Message = res.Error.Message };
				}
				Image image = new()
				{
					AdId = ad.Id,
					Name = res.PublicId,
					Url = res.SecureUri.AbsoluteUri,
					CreatedAt = DateTime.UtcNow,
					IsMain = i == 0,
				};
				images.Add(image);
			}
			await _imageRepository.AddRangeAsync(images);
			await _adRepository.SaveAsync();
			return new ApiResponse { StatusCode = 201, Message = "Ad created successfully!" };
		}
		public async Task<ApiResponse> GetAll()
		{
			var entities = await GetAdsQuery().Where(a => a.ExpireDate > DateTime.Now).ToListAsync();
			var dtos = entities.Select(a => new AdGetDto
			{
				Id = a.Id,
				User = _mapper.Map<UserDto>(a.User),
				Category = _mapper.Map<ChildCategoryDto>(a.Category),
				City = _mapper.Map<CityGetDto>(a.City),
				Name = a.Name,
				Price = a.Price,
				Description = a.Description,
				Images = a.Images.Select(i => new ImageGetDto
				{
					Name = i.Name,
					Url = i.Url,
					IsMain = i.IsMain,
				}).ToList(),
				VideoLink = a.VideoUrl,
				Level = a.Level,
				State = a.State,
			});

			return new ApiResponse { StatusCode = 200, Data = dtos };
		}
		public async Task<ApiResponse> GetAdsByLevel(AdLevel level)
		{
			if (!Enum.TryParse(typeof(AdLevel), level.ToString(), true, out object? res))
			{
				return new ApiResponse { StatusCode = 404 };
			}
			var entities = await GetAdsQuery().Where(a => a.ExpireDate > DateTime.Now && a.Level == level).ToListAsync();
			var dtos = entities.Select(a => new AdGetDto
			{
				Id = a.Id,
				User = _mapper.Map<UserDto>(a.User),
				Category = _mapper.Map<ChildCategoryDto>(a.Category),
				City = _mapper.Map<CityGetDto>(a.City),
				Name = a.Name,
				Price = a.Price,
				Description = a.Description,
				Images = a.Images.Select(i => new ImageGetDto
				{
					Name = i.Name,
					Url = i.Url
				}).ToList(),
				VideoLink = a.VideoUrl,
				Level = a.Level,
				State = a.State,
			});
			return new ApiResponse { StatusCode = 200, Data = dtos };
		}
		public async Task<ApiResponse> GetLatest()
		{
			var entities = await GetAdsQuery().Where(a => a.ExpireDate > DateTime.Now && a.Level == AdLevel.Basic).OrderByDescending(a => a.CreatedAt).ToListAsync();
			var dtos = entities.Select(a => new AdGetDto
			{
				Id = a.Id,
				User = _mapper.Map<UserDto>(a.User),
				Category = _mapper.Map<ChildCategoryDto>(a.Category),
				City = _mapper.Map<CityGetDto>(a.City),
				Name = a.Name,
				Price = a.Price,
				Description = a.Description,
				Images = a.Images.Select(i => new ImageGetDto
				{
					Name = i.Name,
					Url = i.Url
				}).ToList(),
				VideoLink = a.VideoUrl,
				Level = a.Level,
				State = a.State,
			});
			return new ApiResponse { StatusCode = 200, Data = dtos };
		}
		public async Task<ApiResponse> GetById(Guid id)
		{
			var ad = _adRepository.GetAll(a => a.Id == id)
				.Include(a => a.Category)
				.Include(a => a.City)
				.Include(a => a.Images)
				.Include(a => a.User);
			if (ad == null)
			{
				return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
			}
			var dto = await ad.Select(a => new AdGetDto
			{
				User = _mapper.Map<UserDto>(a.User),
				Category = _mapper.Map<ChildCategoryDto>(a.Category),
				City = _mapper.Map<CityGetDto>(a.City),
				Name = a.Name,
				Price = a.Price,
				Description = a.Description,
				Images = a.Images.Select(i => new ImageGetDto
				{
					Name = i.Name,
					Url = i.Url
				}).ToList(),
				VideoLink = a.VideoUrl,
				Level = a.Level,
				State = a.State,
			}).FirstOrDefaultAsync();
			return new ApiResponse { StatusCode = 200, Data = dto };
		}
		public async Task<ApiResponse> GetMyAds(string userId)
		{
			var entities = await _adRepository.GetAll(a => a.UserId == userId)
				.Include(a => a.Category)
				.Include(a => a.City)
				.Include(a => a.Images).ToListAsync();
			var dtos = entities.Select(a => new AdGetDto
			{
				Id = a.Id,
				Name = a.Name,
				Description = a.Description,
				Price = a.Price,
				Level = a.Level,
				State = a.State,
				Status = a.Status,
				User = _mapper.Map<UserDto>(a.User),
				Category = _mapper.Map<ChildCategoryDto>(a.Category),
				City = _mapper.Map<CityGetDto>(a.City),
				VideoLink = a.VideoUrl,
				Images = a.Images.Select(i => new ImageGetDto
				{
					Name = i.Name,
					Url = i.Url,
					IsMain = i.IsMain,
				}).ToList(),

			});
			return new ApiResponse { StatusCode = 200, Data = dtos };
		}
		public async Task<ApiResponse> Update(Guid id, AdPutDto dto)
		{
			var ad = await GetAdsQuery().Where(a => a.Id == id).FirstOrDefaultAsync();
			if (ad == null)
			{
				return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
			}
			ad.FullName = dto.FullName ?? ad.FullName;
			ad.Whatsapp = dto.Whatsapp ?? ad.Whatsapp;
			ad.PhoneNumber = dto.PhoneNumber ?? ad.PhoneNumber;
			ad.Email = dto.Email ?? ad.Email;
			ad.Name = dto.Name ?? ad.Name;
			ad.Description = dto.Description ?? ad.Description;
			ad.Price = dto.Price ?? ad.Price;
			ad.State = dto.State ?? ad.State;
			ad.CategoryId = dto.CategoryId ?? ad.CategoryId;
			ad.CityId = dto.CityId ?? ad.CityId;
			if (dto.Video != null)
			{
				var publicId = ad.VideoUrl.GetFileName();
				var deletionResult = await _mediaService.DeleteMediaAsync(publicId);
				if (deletionResult.Error != null || deletionResult.Result != "ok")
				{
					return new ApiResponse { StatusCode = 500, Message = deletionResult.Error.Message };
				}
				var uploadResult = await _mediaService.VideoUploadAsync(dto.Video);
				if (uploadResult.Error != null)
				{
					return new ApiResponse { StatusCode = 500, Message = uploadResult.Error.Message };
				}
				ad.VideoUrl = uploadResult.SecureUrl.AbsoluteUri;
			}
			if (dto.Files != null)
			{
				var newFileNames = dto.Files.Select(file => Path.GetFileNameWithoutExtension(file.FileName)).ToList();
				var existingPublicIds = ad.Images.Select(image => image.Name).ToList();

				var filesToAdd = newFileNames.Except(existingPublicIds).ToList();
				var filesToRemove = existingPublicIds.Except(filesToAdd).ToList();

				var removedImages = new List<Image>();
				for (int j = 0; j < filesToRemove.Count; j++)
				{
					var res = await _mediaService.DeleteMediaAsync(filesToRemove[j]);
					if (res.Error != null)
					{
						return new ApiResponse { StatusCode = 500, Message = res.Error.Message };
					}
					var image = await _imageRepository.GetAsync(i => i.Name == filesToRemove[j] && !i.IsDeleted);
					removedImages.Add(image);
				}
				_imageRepository.DeleteRange(removedImages);
				await _imageRepository.SaveAsync();
				var newImages = new List<Image>();
				for (int i = 0; i < dto.Files.Count; i++)
				{
					var fileName = Path.GetFileNameWithoutExtension(dto.Files[i].FileName);
					if (filesToAdd.Contains(fileName))
					{
						var res = await _mediaService.ImageUploadAsync(dto.Files[i]);
						if (res.Error != null)
						{
							return new ApiResponse { StatusCode = 500, Message = res.Error.Message };
						}
						Image image = new()
						{
							AdId = ad.Id,
							Name = res.PublicId,
							Url = res.SecureUrl.AbsoluteUri,
							IsMain = i == 0 && !ad.Images.Any(i => i.IsMain),
							CreatedAt = DateTime.Now,
						};
						newImages.Add(image);
					}
				}
				await _imageRepository.AddRangeAsync(newImages);
			}
			ad.LastUpdatedAt = DateTime.Now;
			_adRepository.Update(ad);
			await _adRepository.SaveAsync();
			return new ApiResponse { StatusCode = 204 };
		}
		public async Task<ApiResponse> Delete(Guid id)
		{
			var ad = await _adRepository.GetAsync(a => a.Id == id && !a.IsDeleted);
			if (ad == null)
			{
				return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
			}
			ad.IsDeleted = true;
			await _adRepository.SaveAsync();
			return new ApiResponse { StatusCode = 204 };
		}
		public async Task<ApiResponse> Promote(PromoteDto dto)
		{
			var ad = await GetAdsQuery().Where(a => a.Id == dto.AdId).FirstOrDefaultAsync();
			if (ad == null)
			{
				return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
			}
			var res = await _stripeService.ActivateSubscriptionAsync(dto);
			if (res.StatusCode != 201)
			{
				return new ApiResponse { StatusCode = res.StatusCode, Message = res.Message };
			}
			var metaData = (Dictionary<string, string>)res.Data;
			ad.ExpireDate = DateTime.Now.AddDays(double.Parse(metaData["interval"]));
			ad.IsDeleted = false;
			_adRepository.Update(ad);
			await _adRepository.SaveAsync();
			return new ApiResponse { StatusCode = 204 };
		}
		public async Task<ApiResponse> CancelSubscription(Guid id)
		{
			var ad = await _adRepository.GetAsync(a => a.Id == id && !a.IsDeleted);
			if (ad == null)
			{
				return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
			}
			var res = await _stripeService.CancelSubscription(id);
			return new ApiResponse { StatusCode = res.StatusCode, Message = res.Message };
		}
		public async Task<ApiResponse> AddToFavorites(FavoritePostDto dto)
		{
			Favorite favorites = new()
			{
				AdId = dto.AdId,
				UserId = dto.UserId,
			};
			await _favoriteRepository.AddAsync(favorites);
			await _favoriteRepository.SaveAsync();
			return new ApiResponse { StatusCode = 201, Message = "Added to favorites" };
		}
		public async Task<ApiResponse> RemoveFromFavorites(Guid id)
		{
			var favorite = await _favoriteRepository.GetAsync(f => !f.IsDeleted && f.Id == id);
			if (favorite == null)
			{
				return new ApiResponse { StatusCode = 404 };
			}
			_favoriteRepository.Delete(favorite);
			await _favoriteRepository.SaveAsync();
			return new ApiResponse { StatusCode = 204 };
		}
		public async Task<ApiResponse> DeleteRange(List<Guid> adIds)
		{
			List<Ad> ads = new();
			foreach (var id in adIds)
			{
				var ad = await _adRepository.GetAsync(a => a.Id == id && !a.IsDeleted);
				if (ad == null)
				{
					return new ApiResponse { StatusCode = 404 };
				}
				ads.Add(ad);
			}
			_adRepository.DeleteRange(ads);
			await _adRepository.SaveAsync();
			return new ApiResponse { StatusCode = 204 };
		}
		public async Task<ApiResponse> GetFavorites(string userId)
		{
			var favorites = _favoriteRepository.GetAll(f => f.UserId == userId)
				.Include(f => f.Ad).ThenInclude(a => a.City)
				.Include(f => f.Ad).ThenInclude(a => a.Images);
			var dtos = await favorites.Select(f =>
			new AdGetDto
			{
				Id = f.AdId,
				Name = f.Ad.Name,
				Description = f.Ad.Description,
				Level = f.Ad.Level,
				Price = f.Ad.Price,
				State = f.Ad.State,
				City = new CityGetDto
				{
					Id = f.Ad.CityId,
					Name = f.Ad.City.Name
				},
				Category = new ChildCategoryDto
				{
					Id = f.Ad.CategoryId,
					Name = f.Ad.Category.Name
				},
				VideoLink = f.Ad.VideoUrl,
				Images = f.Ad.Images.Select(i => new ImageGetDto
				{
					Name = i.Name,
					Url = i.Url,
					IsMain = i.IsMain,
				}).ToList()
			}).ToListAsync();
			return new ApiResponse { StatusCode = 200, Data = dtos };
		}
	}
}
