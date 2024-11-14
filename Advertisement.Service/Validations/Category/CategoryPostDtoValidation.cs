using Advertisement.Service.Dtos;
using Advertisement.Service.Extensions;
using FluentValidation;

namespace Advertisement.Service.Validations.Category
{
    public class CategoryPostDtoValidation : AbstractValidator<CategoryPostDto>
    {
        public CategoryPostDtoValidation()
        {
            RuleFor(cp => cp.Name)
                .NotEmpty()
                .NotNull();
            RuleFor(cp => cp.File)
                .NotNull()
                .Custom((file, context) =>
                {
                    if (!file.IsImage())
                    {
                        context.AddFailure("File", "Only files with the following extensions are allowed: png, jpg, jpeg");
                    }
                    if (!file.IsSizeOk(5))
                    {
                        context.AddFailure("File", "File size cannot exceed 5mb");
                    }
                });
        }
    }
}
