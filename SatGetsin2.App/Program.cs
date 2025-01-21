using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SatGetsin2.App.Registration;
using SatGetsin2.Data.Context;
using SatGetsin2.Service.Helpers;
using SatGetsin2.Service.Hubs;
using SatGetsin2.Service.Mappers;
using Stripe;
using System.Text.Json.Serialization;

namespace SatGetsin2.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
			builder.WebHost.UseUrls($"http://*:{port}");

			//builder.Services.AddHealthChecks();
			// Add services to the container.

			builder.Services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
				options.JsonSerializerOptions.WriteIndented = true;
			});
			builder.Services.RegisterValidators();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

			builder.Services
				.RegisterServicesAndRepositories()
				.RegisterJwtServices(builder.Configuration)
				.RegisterIdentity();

			builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			builder.Services.AddDbContext<AppDbContext>(opt =>
			{
				//opt.UseSqlServer(builder.Configuration.GetConnectionString("Deployment"));
				opt.UseSqlServer("Server=sql.bsite.net\\MSSQL2016;Database=satgetsin2_;UID=satgetsin2_;Password=Futurama_07;TrustServerCertificate=true");
			});

			builder.Services.AddFluentValidationAutoValidation()
							.AddFluentValidationClientsideAdapters();
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

			StripeConfiguration.ApiKey = "sk_test_51QISVCG2GGPZx7gnkH03kaHlxxWyfgnUPWWQgW5elJOHq00Z5IMZ3eYolcy1X9qVydLRFnaQckDUZ8w1jNklpJCi00RRiVRkmz";
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapHub<NotificationHub>("/notificationHub");
			app.MapControllers();
			app.UseCors("api");
			//app.UseHealthChecks("/health");
			app.Run();
		}
	}
}
