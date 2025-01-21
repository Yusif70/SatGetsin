using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SatGetsin2.Service.Services.Abstractions;
using Stripe;

namespace SatGetsin2.Service.Services.Concretes
{
	public class UpcomingSubscriptionService : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceProvider;
		public UpcomingSubscriptionService(IServiceScopeFactory serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}
		private HashSet<string> processedSubscriptions = new();
		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				using var scope = _serviceProvider.CreateScope();
				var stripeService = scope.ServiceProvider.GetRequiredService<IStripeService>();
				var res = await stripeService.GetUpcomingSubscriptionsAsync(TimeSpan.FromDays(2));
				var upcomingSubscriptions = (List<Subscription>)res.Data;
				foreach (var subscription in upcomingSubscriptions)
				{
					if (!processedSubscriptions.Contains(subscription.Id))
					{
						var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
						await notificationService.SendNotification(subscription.Metadata["userId"], $"your subscription for ad {subscription.Metadata["adId"]} will be automatically renewed on {subscription.CurrentPeriodEnd}");
						processedSubscriptions.Add(subscription.Id);
					}
				}
				await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
			}
		}
	}
}
