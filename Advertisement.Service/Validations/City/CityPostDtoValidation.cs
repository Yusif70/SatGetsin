using Advertisement.Service.Dtos.City;
using FluentValidation;

namespace Advertisement.Service.Validations.City
{
    public class CityPostDtoValidation : AbstractValidator<CityPostDto>
    {
        public CityPostDtoValidation()
        {
            RuleFor(cp => cp.Name)
                .NotEmpty()
                .NotNull();
        }
    }
}
