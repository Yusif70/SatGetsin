using FluentValidation;
using SatGetsin2.Service.Dtos.City;

namespace SatGetsin2.Service.Validations.City
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
