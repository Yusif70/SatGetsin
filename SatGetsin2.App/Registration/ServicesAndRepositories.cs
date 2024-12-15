﻿using SatGetsin2.Core.Repositories.Abstractions;
using SatGetsin2.Data.Repositories.Concretes;
using SatGetsin2.Service.Services.Abstractions;
using SatGetsin2.Service.Services.Concretes;
using Stripe;

namespace SatGetsin2.App.Registration
{
    public static class ServicesAndRepositories
    {
        public static IServiceCollection RegisterServicesAndRepositories(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IAdRepository, AdRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IAdService, AdService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<CustomerService>();
            services.AddScoped<PaymentIntentService>();
            services.AddScoped<PaymentMethodService>();
            services.AddScoped<StripeService>();

            return services;
        }
    }
}