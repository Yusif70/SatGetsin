using FluentValidation;
using SatGetsin2.Service.Dtos.Auth;

namespace SatGetsin2.Service.Validations.Auth
{
    public class LoginDtoValidation : AbstractValidator<LoginDto>
    {
        public LoginDtoValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull();
        }
    }
}
