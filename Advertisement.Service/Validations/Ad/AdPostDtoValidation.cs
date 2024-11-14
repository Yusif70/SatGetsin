using Advertisement.Service.Dtos.Ad;
using Advertisement.Service.Extensions;
using FluentValidation;

namespace Advertisement.Service.Validations.Ad
{
    public class AdPostDtoValidation : AbstractValidator<AdPostDto>
    {
        public AdPostDtoValidation()
        {
            RuleFor(ap => ap.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();
            RuleFor(ap => ap.FullName)
                .NotEmpty()
                .NotNull();
            RuleFor(ap => ap.PhoneNumber)
                .NotEmpty()
                .NotNull()
                /*.Matches("^(?:0|994)(?:12|51|50|55|70|77)[\\\\s.-]?[2-9][0-9]{2}[\\\\s.-]?[0-9]{2}[\\\\s.-]?[0-9]{2}$\r\n").WithMessage("Phone number not valid")*/;
            RuleFor(ap => ap.Whatsapp)
                .NotEmpty()
                .NotNull()
                /*.Matches("(?:0|994)(?:12|51|50|55|70|77)[^\\w]{0,2}[2-9][0-9]{2}[^\\w]{0,2}[0-9]{2}[^\\w]{0,2}[0-9]{2}").WithMessage("Phone number not valid")*/;
            RuleFor(ap => ap.Name)
                .NotEmpty()
                .NotNull();
            RuleFor(ap => ap.State)
                .NotNull();
            RuleFor(ap => ap.Price)
                .NotEmpty()
                .NotNull();
            RuleFor(ap => ap.Description)
                .NotEmpty()
                .NotNull();
            RuleFor(ap => ap.VideoLink)
                .NotEmpty()
                .NotNull();
            RuleFor(ap => ap.Files)
                .NotNull()
                .Custom((files, context) =>
                {
                    foreach (var file in files)
                    {
                        if (!file.IsImage())
                        {
                            context.AddFailure("File", "Only files with the following extensions are allowed: png, jpg, jpeg");
                        }
                        if (!file.IsSizeOk(5))
                        {
                            context.AddFailure("File", "File size cannot exceed 5mb");
                        }
                    }
                });
        }
    }
}
