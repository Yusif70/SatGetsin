﻿using FluentValidation;
using SatGetsin2.Service.Dtos.Category;
using SatGetsin2.Service.Extensions;

namespace SatGetsin2.Service.Validations.Category
{
    public class CategoryPutDtoValidation : AbstractValidator<CategoryPutDto>
    {
        public CategoryPutDtoValidation()
        {
            RuleFor(cp => cp.File)
                .Custom((file, context) =>
                {
                    if (!file.IsImage())
                    {
                        context.AddFailure("File", "Invalid image file");
                    }
                    if (!file.IsSizeOk(5))
                    {
                        context.AddFailure("File", "File size cannot exceed 5mb");
                    }
                }).When(cp => cp.File != null);
        }
    }
}
