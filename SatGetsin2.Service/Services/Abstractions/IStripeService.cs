using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.Ad;

namespace SatGetsin2.Service.Services.Abstractions
{
	public interface IStripeService
	{
		Task<ApiResponse> ActivateSubscriptionAsync(PromoteDto dto);
		Task<ApiResponse> CancelSubscription(Guid adId);
		Task<ApiResponse> CreateProducts();
		Task<ApiResponse> GetUpcomingSubscriptionsAsync(TimeSpan remaining);
	}
}
