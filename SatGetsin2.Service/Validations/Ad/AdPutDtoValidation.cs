using FluentValidation;
using SatGetsin2.Service.Dtos.Ad;
using SatGetsin2.Service.Extensions;

namespace SatGetsin2.Service.Validations.Ad
{
    public class AdPutDtoValidation : AbstractValidator<AdPutDto>
    {
        public AdPutDtoValidation()
        {
            RuleFor(ap => ap.Email)
                .EmailAddress().When(ap => ap.Email != null);
            RuleFor(ap => ap.PhoneNumber)
                .Matches("^(?:\\+994|0)(50|51|55|70|77|99|10)\\d{7}$");
            RuleFor(ap => ap.Whatsapp)
                .Matches("^(?:\\+994|0)(50|51|55|70|77|99|10)\\d{7}$");
            RuleFor(ap => ap.Name)
                .MaximumLength(5000).When(ap => !string.IsNullOrEmpty(ap.Name));
            RuleFor(ap => ap.Price)
                .GreaterThan(0).When(ap => ap.Price != null);
            RuleFor(ap => ap.Description)
                .MaximumLength(5000).When(ap => !string.IsNullOrEmpty(ap.Description));
            RuleFor(ap => ap.Video)
                .Custom((video, context) =>
                {
                    if (!video.IsVideo())
                    {
                        context.AddFailure("File", "Invalid video file");
                    }
                }).When(ap => ap.Video != null);
            RuleFor(ap => ap.Files)
                .Must(files => files.Count < 5 || files == null).WithMessage("Image count must be less than 5")
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
                }).When(ap => ap.Files != null);
        }
    }
}
