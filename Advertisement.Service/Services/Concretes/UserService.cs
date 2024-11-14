using Advertisement.Core.Entities;
using Advertisement.Service.ApiResponses;
using Microsoft.AspNetCore.Identity;

namespace Advertisement.Service.Services.Concretes
{
    public class UserService
    {
        private readonly PaymentService _paymentService;
        private readonly UserManager<AppUser> _userManager;

        public UserService(PaymentService paymentService, UserManager<AppUser> userManager)
        {
            _paymentService = paymentService;
            _userManager = userManager;
        }
        public async Task<ApiResponse> TopUpBalance(string userId, double amount, string currency = "azn")
        {

            var success = await _paymentService.ProcessPaymentAsync(userId, amount, currency);

            if (!success)
            {
                return new ApiResponse { StatusCode = 500 };
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse { StatusCode = 404 };
            }
            user.Balance += amount;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
            {
                return new ApiResponse { StatusCode = 500 };
            }
            return new ApiResponse { StatusCode = 200, Message = "Top-up successful" };
        }
    }
}
