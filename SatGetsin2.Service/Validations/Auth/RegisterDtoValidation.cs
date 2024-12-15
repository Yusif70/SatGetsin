using SatGetsin2.Service.Dtos.Auth;
using FluentValidation;

namespace SatGetsin2.Service.Validations.Auth
{
    public class RegisterDtoValidation : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidation()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.FullName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50);
            RuleFor(r => r.Email)
                .NotNull()
                .EmailAddress();
            RuleFor(r => r.Password)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50);
            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .NotNull();
        }
    }
}
