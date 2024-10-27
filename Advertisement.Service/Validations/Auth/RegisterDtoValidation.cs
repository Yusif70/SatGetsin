using Advertisement.Service.Dtos.Auth;
using FluentValidation;

namespace Advertisement.Service.Validations.Auth
{
    public class RegisterDtoValidation : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidation()
        {
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
            RuleFor(r => r.Code)
                .NotEmpty()
                .NotNull();
            RuleFor(r => r.AgreedToTerms)
                .Equal(true);
        }
    }
}
