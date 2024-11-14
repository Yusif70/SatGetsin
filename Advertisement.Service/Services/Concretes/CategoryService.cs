using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos;
using Advertisement.Service.Services.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Advertisement.Service.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IPhotoService photoService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<ApiResponse> Create(CategoryPostDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            if (dto.File != null)
            {
                var res = await _photoService.AddPhotoAsync(dto.File);
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
            var entities = await _categoryRepository.GetAll(x => !x.IsDeleted).ToListAsync();
            var dtos = entities.Select(c => new CategoryGetDto
            {
                Name = c.Name,
                Id = c.Id,
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
            var category = await _categoryRepository.GetAll(x => !x.IsDeleted && x.Id == id).ToListAsync();
            if (category == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Category not found" };
            }
            var dto = category.Select(c => new CategoryGetDto
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                ChildCategories = c.ChildCategories?.Select(c => new ChildCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            }).FirstOrDefault();
            return new ApiResponse { StatusCode = 200, Data = dto };
        }

        public async Task<ApiResponse> Update(Guid id, CategoryPutDto dto)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == id && !c.IsDeleted);
            if (category == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Category not found" };
            }
            category.Name = dto.Name;
            category.ParentCategoryId = dto.ParentCategoryId;
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
