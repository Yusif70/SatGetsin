using Microsoft.AspNetCore.Identity;
using SatGetsin2.Core.Entities;
using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.User;
using SatGetsin2.Service.Extensions;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.Service.Services.Concretes
{
	public class UserService : IUserService
	{
		private readonly IStripeService _stripeService;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMediaService _photoService;

		public UserService(IStripeService stripeService, UserManager<AppUser> userManager, IMediaService photoService)
		{
			_stripeService = stripeService;
			_userManager = userManager;
			_photoService = photoService;
		}
		//public async Task<ApiResponse> ChargeCard()
		//{
		//    var res = await _stripeService.CreatePaymentMethodAsync();
		//    return new ApiResponse { StatusCode = 200, Data = res };
		//}
		//public async Task<ApiResponse> TopUpBalance(string paymentMethodId, string userId, double amount, string currency = "azn")
		//{
		//	var user = await _userManager.FindByIdAsync(userId);
		//	if (user == null)
		//	{
		//		return new ApiResponse { StatusCode = 401 };
		//	}
		//	if (amount <= 0)
		//	{
		//		return new ApiResponse { StatusCode = 400, Message = "Amount must be greater than zero" };
		//	}
		//	var paymentResult = await _stripeService.CreatePaymentIntentAsync(paymentMethodId, user.FullName, amount, user.Email, currency);
		//	if (!paymentResult.Succeeded)
		//	{
		//		return new ApiResponse { StatusCode = 500, Message = paymentResult.ErrorMessage };
		//	}
		//	var confirmation = await _stripeService.ConfirmPaymentIntentAsync(paymentResult.PaymentIntentId);
		//	if (!confirmation.Succeeded)
		//	{
		//		return new ApiResponse { StatusCode = 500, Message = confirmation.ErrorMessage };
		//	}
		//	user.Balance += amount;
		//	await _userManager.UpdateAsync(user);
		//	return new ApiResponse { StatusCode = 200, Message = "Top-up successful" };
		//}
		public async Task<ApiResponse> UpdatePfp(UpdatePfpDto dto)
		{
			var user = await _userManager.FindByIdAsync(dto.UserId);
			if (user == null)
			{
				return new ApiResponse { StatusCode = 404 };
			}
			if (user.Pfp != null)
			{
				var publicId = user.Pfp.GetFileName();
				var deletionResult = await _photoService.DeleteMediaAsync(publicId);
				if (deletionResult.Error != null)
				{
					return new ApiResponse { StatusCode = 500, Message = deletionResult.Error.Message };
				}
			}
			var imageUploadResult = await _photoService.ImageUploadAsync(dto.File);
			if (imageUploadResult.Error != null)
			{
				return new ApiResponse { StatusCode = 500, Message = imageUploadResult.Error.Message };
			}
			user.Pfp = imageUploadResult.SecureUrl.AbsoluteUri;
			var identityResult = await _userManager.UpdateAsync(user);
			if (!identityResult.Succeeded)
			{
				return new ApiResponse { StatusCode = 500, Data = identityResult.Errors.Select(e => e.Description) };
			}
			return new ApiResponse { StatusCode = 204 };
		}
	}
}
