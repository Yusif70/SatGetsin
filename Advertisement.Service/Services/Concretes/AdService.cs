using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos;
using Advertisement.Service.Dtos.Ad;
using Advertisement.Service.Dtos.City;
using Advertisement.Service.Dtos.Image;
using Advertisement.Service.Helpers;
using Advertisement.Service.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Advertisement.Service.Services.Concretes
{
    public class AdService : IAdService
    {
        private readonly IMapper _mapper;
        private readonly IAdRepository _adRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IFavoritesRepository _favoritesRepository;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;
        public AdService(IMapper mapper, IAdRepository adRepository, IImageRepository imageRepository, IPhotoService photoService, UserManager<AppUser> userManager, IFavoritesRepository favoritesRepository)
        {
            _mapper = mapper;
            _adRepository = adRepository;
            _imageRepository = imageRepository;
            _photoService = photoService;
            _userManager = userManager;
            _favoritesRepository = favoritesRepository;
        }

        private IQueryable<Ad> GetAdsQuery()
        {
            return _adRepository.GetAll(a => !a.IsDeleted)
                .Include(a => a.Category)
                .Include(a => a.City)
                .Include(a => a.Images)
                .Include(a => a.User);
        }
        public async Task<ApiResponse> Create(AdPostDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return new ApiResponse { StatusCode = 404 };
            }
            var ad = _mapper.Map<Ad>(dto);
            ad.Level = Core.Enums.AdLevel.Basic;
            ad.CreatedAt = DateTime.Now;
            ad.ExpireDate = DateTime.Now.AddDays(30);
            await _adRepository.AddAsync(ad);
            foreach (var file in dto.Files)
            {
                var res = await _photoService.AddPhotoAsync(file);
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
                };
                await _imageRepository.AddAsync(image);
            }
            await _adRepository.SaveAsync();
            return new ApiResponse { StatusCode = 201, Message = "Ad created successfully!" };
        }
        public async Task<ApiResponse> GetAll()
        {
            var entities = await GetAdsQuery().Where(a => a.ExpireDate > DateTime.Now).ToListAsync();
            var dtos = entities.Select(a => new AdGetDto
            {
                User = _mapper.Map<UserDto>(a.User),
                Category = _mapper.Map<CategoryGetDto>(a.Category),
                City = _mapper.Map<CityGetDto>(a.City),
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                Whatsapp = a.Whatsapp,
                Email = a.Email,
                Name = a.Name,
                Price = a.Price,
                Description = a.Description,
                Images = a.Images.Select(i => new ImageGetDto
                {
                    Url = i.Url
                }).ToList(),
                VideoLink = a.VideoLink,
                Level = a.Level,
                State = a.State,
            });

            return new ApiResponse { StatusCode = 200, Data = dtos };
        }
        // TODO: generic method for premium and vip ads
        public async Task<ApiResponse> GetPremium()
        {
            var entities = await GetAdsQuery().Where(a => a.ExpireDate < DateTime.Now && a.Level == Core.Enums.AdLevel.Premium).ToListAsync();
            var dtos = entities.Select(a => new AdGetDto
            {
                Category = _mapper.Map<CategoryGetDto>(a.Category),
                City = _mapper.Map<CityGetDto>(a.City),
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                Whatsapp = a.Whatsapp,
                Email = a.Email,
                Name = a.Name,
                Price = a.Price,
                Description = a.Description,
                Images = a.Images.Select(i => new ImageGetDto
                {
                    Url = i.Url
                }).ToList(),
                VideoLink = a.VideoLink,
                Level = a.Level,
                State = a.State,
            });
            return new ApiResponse { StatusCode = 200, Data = dtos };
        }
        public async Task<ApiResponse> GetVip()
        {
            var entities = await GetAdsQuery().Where(a => a.ExpireDate < DateTime.Now && a.Level == Core.Enums.AdLevel.Vip).ToListAsync();
            var dtos = entities.Select(a => new AdGetDto
            {
                Category = _mapper.Map<CategoryGetDto>(a.Category),
                City = _mapper.Map<CityGetDto>(a.City),
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                Whatsapp = a.Whatsapp,
                Email = a.Email,
                Name = a.Name,
                Price = a.Price,
                Description = a.Description,
                Images = a.Images.Select(i => new ImageGetDto
                {
                    Url = i.Url
                }).ToList(),
                VideoLink = a.VideoLink,
                Level = a.Level,
                State = a.State,
            });
            return new ApiResponse { StatusCode = 200, Data = dtos };
        }
        public async Task<ApiResponse> GetLatest()
        {
            var entities = await GetAdsQuery().Where(a => a.ExpireDate < DateTime.Now && a.Level == Core.Enums.AdLevel.Basic).OrderByDescending(a => a.CreatedAt).ToListAsync();
            var dtos = entities.Select(a => new AdGetDto
            {
                Category = _mapper.Map<CategoryGetDto>(a.Category),
                City = _mapper.Map<CityGetDto>(a.City),
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                Whatsapp = a.Whatsapp,
                Email = a.Email,
                Name = a.Name,
                Price = a.Price,
                Description = a.Description,
                Images = a.Images.Select(i => new ImageGetDto
                {
                    Url = i.Url
                }).ToList(),
                VideoLink = a.VideoLink,
                Level = a.Level,
                State = a.State,
            });
            return new ApiResponse { StatusCode = 200, Data = dtos };
        }
        public async Task<ApiResponse> GetById(Guid id)
        {
            var ad = await GetAdsQuery().Where(a => a.Id == id).ToListAsync();
            if (ad == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
            }
            var dto = ad.Select(a => new AdGetDto
            {
                Category = _mapper.Map<CategoryGetDto>(a.Category),
                City = _mapper.Map<CityGetDto>(a.City),
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                Whatsapp = a.Whatsapp,
                Email = a.Email,
                Name = a.Name,
                Price = a.Price,
                Description = a.Description,
                Images = a.Images.Select(i => new ImageGetDto
                {
                    Name = i.Name,
                    Url = i.Url
                }).ToList(),
                VideoLink = a.VideoLink,
                Level = a.Level,
                State = a.State,
            }).FirstOrDefault();
            return new ApiResponse { StatusCode = 200, Data = dto };
        }
        public async Task<ApiResponse> Update(Guid id, AdPutDto dto)
        {
            var ad = await GetAdsQuery().Where(a => a.Id == id).FirstOrDefaultAsync();
            if (ad == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
            }
            ad.FullName = dto.FullName;
            ad.Whatsapp = dto.Whatsapp;
            ad.PhoneNumber = dto.PhoneNumber;
            ad.Email = dto.Email;
            ad.Name = dto.Name;
            ad.Description = dto.Description;
            ad.Price = dto.Price;
            ad.State = dto.State;
            ad.VideoLink = dto.VideoLink;
            ad.CategoryId = dto.CategoryId;
            ad.CityId = dto.CityId;
            if (dto.Files != null)
            {
                var newFileNames = dto.Files.Select(file => Path.GetFileNameWithoutExtension(file.FileName)).ToList();
                var existingPublicIds = ad.Images.Select(image => CloudinaryHelper.GetCloudinaryPublicId(image.Url)).ToList();

                var filesToAdd = newFileNames.Except(existingPublicIds).ToList();
                var filesToRemove = existingPublicIds.Except(filesToAdd).ToList();
                foreach (var file in dto.Files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    if (filesToAdd.Contains(fileName))
                    {
                        var res = await _photoService.AddPhotoAsync(file);
                        if (res.Error != null)
                        {
                            return new ApiResponse { StatusCode = 500, Message = res.Error.Message };
                        }
                        Image image = new()
                        {
                            AdId = ad.Id,
                            Name = res.PublicId,
                            Url = res.SecureUri.AbsoluteUri,
                            CreatedAt = DateTime.Now,
                        };
                        await _imageRepository.AddAsync(image);
                    }
                }
                foreach (var publicId in filesToRemove)
                {
                    var res = await _photoService.DeletePhotoAsync(publicId);
                    if (res.Error != null)
                    {
                        return new ApiResponse { StatusCode = 500, Message = res.Error.Message };
                    }
                    var image = await _imageRepository.GetAsync(i => i.Name == publicId && !i.IsDeleted);
                    _imageRepository.Delete(image);
                }
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
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user.Balance < 1)
            {
                return new ApiResponse { StatusCode = 400, Message = "Insufficient funds" };
            }
            ad.ExpireDate = DateTime.Now.AddDays(10);
            _adRepository.Update(ad);
            user.Balance -= 1;
            await _userManager.UpdateAsync(user);
            await _adRepository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }
        public async Task<ApiResponse> AddToFavorites(FavoriteDto dto)
        {
            Favorites favorites = new()
            {
                AdId = dto.AdId,
                UserId = dto.UserId,
            };
            await _favoritesRepository.AddAsync(favorites);
            await _favoritesRepository.SaveAsync();
            return new ApiResponse { StatusCode = 201, Message = "Added to favorites" };
        }
        public async Task<ApiResponse> RemoveFromFavorites(Guid id)
        {
            var favorite = await _favoritesRepository.GetAsync(f => !f.IsDeleted && f.Id == id);
            if (favorite == null)
            {
                return new ApiResponse { StatusCode = 404 };
            }
            _favoritesRepository.Delete(favorite);
            await _favoritesRepository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }
    }
}
