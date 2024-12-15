using FluentValidation;
using SatGetsin2.Service.Dtos.Ad;
using SatGetsin2.Service.Extensions;

namespace SatGetsin2.Service.Validations.Ad
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
                .Matches("^(?:\\+994|0)(50|51|55|70|77|99|10)\\d{7}$");
            RuleFor(ap => ap.Whatsapp)
                .NotEmpty()
                .NotNull()
                .Matches("^(?:\\+994|0)(50|51|55|70|77|99|10)\\d{7}$");
            RuleFor(ap => ap.Name)
                .NotEmpty()
                .NotNull();
            RuleFor(ap => ap.State)
                .NotNull();
            RuleFor(ap => ap.Price)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);
            RuleFor(ap => ap.Description)
                .NotEmpty()
                .NotNull()
                .MaximumLength(5000);
            RuleFor(ap => ap.Video)
                .NotNull()
                .Custom((video, context) =>
                {
                    if (!video.IsVideo())
                    {
                        context.AddFailure("File", "Invalid video file");
                    }
                });
            RuleFor(ap => ap.Files)
                .NotNull()
                .Must(files => files.Count < 5).WithMessage("Image count must be less than 5")
                .Custom((files, context) =>
                {
                    foreach (var file in files)
                    {
                        if (file != null)
                        {
                            if (!file.IsImage())
                            {
                                context.AddFailure("File", "Invalid image file");
                            }
                            if (!file.IsSizeOk(5))
                            {
                                context.AddFailure("File", "File size cannot exceed 5mb");
                            }
                        }
                    }
                });
        }
    }
}
