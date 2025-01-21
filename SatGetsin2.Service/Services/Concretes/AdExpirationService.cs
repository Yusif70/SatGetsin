using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SatGetsin2.Service.Dtos.Ad;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.Service.Services.Concretes
{
	public class AdExpirationService : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceProvider;
		public AdExpirationService(IServiceScopeFactory serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		private HashSet<Guid> processedAds = new();
		protected override async Task ExecuteAsync(CancellationToken token)
		{
			while (token.IsCancellationRequested)
			{
				using var scope = _serviceProvider.CreateScope();
				var adService = scope.ServiceProvider.GetRequiredService<AdService>();
				var res = await adService.GetAll();
				var ads = (List<AdGetDto>)res.Data;
				foreach (var ad in ads)
				{
					if (ad.ExpireDate < DateTime.Now && !processedAds.Contains(ad.Id))
					{
						await adService.Delete(ad.Id);
						var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
						await notificationService.SendNotification(ad.User.Id, $"ad {ad.Id} expired");
						processedAds.Add(ad.Id);
					}
				}
				await Task.Delay(TimeSpan.FromMinutes(1), token);
			}
		}
	}
}
