namespace SatGetsin2.Service.Services.Abstractions
{
	public interface INotificationService
	{
		Task SendNotification(string userId, string message);
	}
}
