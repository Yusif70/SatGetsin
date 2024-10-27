using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.Category;
using Advertisement.Service.Extensions;
using Advertisement.Service.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Advertisement.Service.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPhotoService _photoService;

        public CategoryService(ICategoryRepository repository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor, IPhotoService photoService)
        {
            _repository = repository;
            _mapper = mapper;
            _env = webHostEnvironment;
            _contextAccessor = contextAccessor;
            _photoService = photoService;
        }

        public async Task<ApiResponse> Create(CategoryPostDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            if (dto.File != null)
            {
                var res = await _photoService.AddPhotoAsync(dto.File);
                if (res.Error != null)
                {
                    return new ApiResponse { StatusCode = 400, Message = res.Error.Message };
                }
                entity.ImageUrl = res.SecureUrl.AbsoluteUri;
                string root = _env.WebRootPath;
                string path = "assets/img/categories";
                entity.Image = await dto.File.SaveAsync(root, path);
                var req = _contextAccessor.HttpContext.Request;
                //entity.ImageUrl = $"{req.Scheme}://{req.Host}/assets/img/categories/{entity.Image}";
            }
            entity.CreatedAt = DateTime.Now;
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
            return new ApiResponse { StatusCode = 201, Message = "Category created successfully!" };
        }

        public async Task<ApiResponse> GetAll()
        {
            var entities = await _repository.GetAll(c => !c.IsDeleted).Include(c => c.ChildCategories).ToListAsync();
            return new ApiResponse { StatusCode = 200, Data = entities };
        }

        public async Task<ApiResponse> GetById(Guid id)
        {
            var entity = await _repository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (entity == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Category not found" };
            }
            var dto = _mapper.Map<CategoryGetDto>(entity);
            return new ApiResponse { StatusCode = 200, Data = dto };
        }

        public async Task<ApiResponse> Update(Guid id, CategoryPutDto dto)
        {
            var entity = await _repository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (entity == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Category not found" };
            }
            entity.Name = dto.Name;
            entity.ParentCategoryId = dto.ParentCategoryId;
            if (dto.File != null)
            {
                string root = _env.WebRootPath;
                string path = "assets/img/categories";
                string oldFilePath = Path.Combine(root, path, entity.Image);
                File.Delete(oldFilePath);
                entity.Image = await dto.File.SaveAsync(root, path);
                var req = _contextAccessor.HttpContext.Request;
                entity.ImageUrl = $"{req.Scheme}://{req.Host}/assets/img/categories/{entity.Image}";
            }
            entity.LastUpdatedAt = DateTime.Now;
            _repository.Update(entity);
            await _repository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }

        public async Task<ApiResponse> Delete(Guid id)
        {
            var entity = await _repository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (entity == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Category not found" };
            }
            entity.IsDeleted = true;
            await _repository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }
    }
}
