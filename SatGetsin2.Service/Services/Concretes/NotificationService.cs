using Microsoft.AspNetCore.SignalR;
using SatGetsin2.Service.Hubs;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.Service.Services.Concretes
{
	public class NotificationService : INotificationService
	{
		private readonly IHubContext<NotificationHub> _hubContext;

		public NotificationService(IHubContext<NotificationHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task SendNotification(string userId, string message)
		{
			await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
		}
	}
}
