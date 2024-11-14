using Advertisement.Core.Entities;
using Advertisement.Core.Repositories.Abstractions;
using Advertisement.Data.Context;
using Advertisement.Data.Repositories.Concretes;
using Advertisement.Service.Helpers;
using Advertisement.Service.Mappers;
using Advertisement.Service.Services.Abstractions;
using Advertisement.Service.Services.Concretes;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Advertisement.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            }); ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the next input below. \r\n\r\nExample: \"Bearer qwewqwewq\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                 });
            });

            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<IAdRepository, AdRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IAdService, AdService>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<ISmsService, SmsService>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();

            builder.Services.AddIdentity<AppUser, IdentityRole>()
            .AddSignInManager<CustomSignInManager<AppUser>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredUniqueChars = 1;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 6;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;

                opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-._@+";
                opt.User.RequireUniqueEmail = true;

                //opt.SignIn.RequireConfirmedEmail = true;
                //opt.SignIn.RequireConfirmedPhoneNumber = true;
            });

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWT:issuer"],
                    ValidAudience = builder.Configuration["JWT:audience"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:secret_key"]))
                };
            });
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Deployment"));
            });

            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddAutoMapper(typeof(CategoryMapper));

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("api", option =>
                {
                    option.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCors("api");
            //app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
