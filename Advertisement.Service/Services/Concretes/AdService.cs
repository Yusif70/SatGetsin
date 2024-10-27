using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.Ad;
using Advertisement.Service.Extensions;
using Advertisement.Service.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Advertisement.Service.Services.Concretes
{
    public class AdService : IAdService
    {
        private readonly IMapper _mapper;
        private readonly IAdRepository _repository;
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPhotoService _photoService;
        public AdService(IMapper mapper, IAdRepository repository, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment, IImageRepository imageRepository, IPhotoService photoService)
        {
            _mapper = mapper;
            _repository = repository;
            _contextAccessor = contextAccessor;
            _env = webHostEnvironment;
            _imageRepository = imageRepository;
            _photoService = photoService;
        }
        private IQueryable<Ad> GetAdsQuery()
        {
            return _repository.GetAll(a => !a.IsDeleted)
                              .Include(a => a.City)
                              .Include(a => a.Images);
        }
        public async Task<ApiResponse> Create(AdPostDto dto)
        {
            var entity = _mapper.Map<Ad>(dto);
            string root = _env.WebRootPath;
            string path = "assets/img/ads";
            //var req = _contextAccessor.HttpContext.Request;
            entity.Level = Core.Enums.AdLevel.Basic;
            entity.CreatedAt = DateTime.Now;
            await _repository.AddAsync(entity);
            foreach (var file in dto.Files)
            {
                string imageName = await file.SaveAsync(root, path);
                //entity.Images.Add(image);
                //string imageUrl = $"{req.Scheme}://{req.Host}/assets/img/ads/{imageName}";
                //entity.ImageUrls.Add(imageUrl);
                var res = await _photoService.AddPhotoAsync(file);
                if (res.Error != null)
                {
                    return new ApiResponse { StatusCode = 400, Message = res.Error.Message };
                }
                Image image = new()
                {
                    AdId = entity.Id,
                    Name = imageName,
                    Url = res.SecureUrl.AbsoluteUri,
                    CreatedAt = DateTime.UtcNow,
                };
                await _imageRepository.AddAsync(image);
            }
            await _repository.SaveAsync();
            return new ApiResponse { StatusCode = 201, Message = "Ad created successfully!" };
        }

        public async Task<ApiResponse> GetAll()
        {
            var entities = await GetAdsQuery().ToListAsync();
            return new ApiResponse { StatusCode = 200, Data = entities };
        }
        public async Task<ApiResponse> GetPremium()
        {
            var entities = await GetAdsQuery().Where(a => a.Level == Core.Enums.AdLevel.Premium).ToListAsync();
            return new ApiResponse { StatusCode = 200, Data = entities };
        }
        public async Task<ApiResponse> GetVip()
        {
            var entities = await GetAdsQuery().Where(a => a.Level == Core.Enums.AdLevel.Vip).ToListAsync();
            return new ApiResponse { StatusCode = 200, Data = entities };
        }
        public async Task<ApiResponse> GetLatest()
        {
            var entities = await GetAdsQuery().OrderByDescending(a => a.CreatedAt).ToListAsync();
            return new ApiResponse { StatusCode = 200, Data = entities };
        }
        public async Task<ApiResponse> GetById(Guid id)
        {
            var entity = await _repository.GetAsync(a => a.Id == id && !a.IsDeleted);
            if (entity == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
            }
            var dto = _mapper.Map<AdGetDto>(entity);
            return new ApiResponse { StatusCode = 200, Data = dto };
        }

        //public async Task<ApiResponse> Update(Guid id, AdPutDto dto)
        //{
        //    var entity = await _repository.GetAsync(a => a.Id == id && !a.IsDeleted);
        //    if (entity == null)
        //    {
        //        return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
        //    }
        //    entity.Name = dto.Name;
        //    entity.Description = dto.Description;
        //    if (dto.Files != null)
        //    {
        //        string root = _env.WebRootPath;
        //        string path = "assets/img/ads";
        //        //foreach (var image in entity.Images)
        //        //{
        //        //    string oldFilePath = Path.Combine(root, path, image);
        //        //    File.Delete(oldFilePath);
        //        //    var req = _contextAccessor.HttpContext.Request;
        //        //    foreach (var file in dto.Files)
        //        //    {
        //        //        string newImage = await file.SaveAsync(root, path);
        //        //        entity.Images.Add(newImage);
        //        //        entity.ImageUrls.Add($"{req.Scheme}://{req.Host}/assets/img/ads/{newImage}");
        //        //    }
        //        //}

        //        //entity.Image = await dto.File.SaveAsync(root, path);
        //        //entity.ImageUrl = $"{req.Scheme}://{req.Host}/assets/img/ads/{entity.Image}";
        //    }
        //    entity.LastUpdatedAt = DateTime.Now;
        //    _repository.Update(entity);
        //    await _repository.SaveAsync();
        //    return new ApiResponse { StatusCode = 204 };
        //}

        public async Task<ApiResponse> Delete(Guid id)
        {
            var entity = await _repository.GetAsync(a => a.Id == id && !a.IsDeleted);
            if (entity == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Ad not found" };
            }
            entity.IsDeleted = true;
            await _repository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }
    }
}
