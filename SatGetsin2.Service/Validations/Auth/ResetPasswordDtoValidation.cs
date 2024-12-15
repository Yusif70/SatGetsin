using SatGetsin2.Service.Dtos.Auth;
using FluentValidation;

namespace SatGetsin2.Service.Validations.Auth
{
    public class ResetPasswordDtoValidation : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidation()
        {
            RuleFor(rpd => rpd.NewPassword)
                .NotEmpty()
                .NotNull();
            RuleFor(rpd => rpd.ConfirmPassword)
                .NotEmpty()
                .NotNull();
            RuleFor(rpd => rpd)
                .Custom((rpd, context) =>
                {
                    if (rpd.ConfirmPassword != rpd.NewPassword)
                    {
                        context.AddFailure(nameof(rpd.ConfirmPassword), "Passwords do not match");
                    }
                });
        }
    }
}
