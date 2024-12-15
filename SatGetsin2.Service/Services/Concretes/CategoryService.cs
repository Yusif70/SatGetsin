using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SatGetsin2.Core.Entities;
using SatGetsin2.Core.Repositories.Abstractions;
using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.Category;
using SatGetsin2.Service.Extensions;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.Service.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IMediaService mediaService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _mediaService = mediaService;
        }
        public async Task<ApiResponse> Create(CategoryPostDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            if (dto.File != null)
            {
                var res = await _mediaService.ImageUploadAsync(dto.File);
                if (res.Error != null)
                {
                    return new ApiResponse { StatusCode = 400, Message = res.Error.Message };
                }
                category.ImageUrl = res.SecureUrl.AbsoluteUri;
            }
            category.CreatedAt = DateTime.Now;
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveAsync();
            return new ApiResponse { StatusCode = 201, Message = "Category created successfully!" };
        }

        public async Task<ApiResponse> GetAll()
        {
            var entities = await _categoryRepository.GetAll(c => !c.IsDeleted).ToListAsync();
            var dtos = entities.Select(c => new CategoryGetDto
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                ChildCategories = c.ChildCategories?.Select(c => new ChildCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            });

            return new ApiResponse { StatusCode = 200, Data = dtos };
        }

        public async Task<ApiResponse> GetById(Guid id)
        {
            var category = await _categoryRepository.GetAll(c => !c.IsDeleted).Include(c => c.ChildCategories).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Category not found" };
            }
            var dto = new CategoryGetDto
            {
                Name = category.Name,
                ImageUrl = category.ImageUrl,
                ChildCategories = category.ChildCategories?.Select(c => new ChildCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };
            return new ApiResponse { StatusCode = 200, Data = dto };
        }

        public async Task<ApiResponse> Update(Guid id, CategoryPutDto dto)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (category == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Category not found" };
            }
            category.Name = dto.Name ?? category.Name;
            category.ParentCategoryId = dto.ParentCategoryId ?? category.ParentCategoryId;
            if (dto.File != null)
            {
                var publicId = category.ImageUrl.GetFileName();
                var deletionResult = await _mediaService.DeleteMediaAsync(publicId);
                if (deletionResult.Error != null || deletionResult.Result != "ok")
                {
                    return new ApiResponse { StatusCode = 500, Message = deletionResult.Error.Message };
                }
                var uploadResult = await _mediaService.ImageUploadAsync(dto.File);
                if (uploadResult.Error != null)
                {
                    return new ApiResponse { StatusCode = 400, Message = uploadResult.Error.Message };
                }
                category.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
            }
            category.LastUpdatedAt = DateTime.Now;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }

        public async Task<ApiResponse> Delete(Guid id)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (category == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Category not found" };
            }
            category.IsDeleted = true;
            await _categoryRepository.SaveAsync();
            return new ApiResponse { StatusCode = 204 };
        }
    }
}
