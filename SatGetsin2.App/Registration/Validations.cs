using FluentValidation;
using SatGetsin2.Service.Validations.Ad;
using SatGetsin2.Service.Validations.Auth;
using SatGetsin2.Service.Validations.Category;
using SatGetsin2.Service.Validations.City;

namespace SatGetsin2.App.Registration
{
    public static class Validations
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services
                .AddValidatorsFromAssemblyContaining<AdPostDtoValidation>()
                .AddValidatorsFromAssemblyContaining<AdPutDtoValidation>()
                .AddValidatorsFromAssemblyContaining<CategoryPostDtoValidation>()
                .AddValidatorsFromAssemblyContaining<CategoryPutDtoValidation>()
                .AddValidatorsFromAssemblyContaining<CityPostDtoValidation>()
                .AddValidatorsFromAssemblyContaining<LoginDtoValidation>()
                .AddValidatorsFromAssemblyContaining<RegisterDtoValidation>()
                .AddValidatorsFromAssemblyContaining<ResetPasswordDtoValidation>();

            return services;
        }
    }
}
