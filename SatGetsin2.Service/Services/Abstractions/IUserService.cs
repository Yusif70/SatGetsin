using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.User;

namespace SatGetsin2.Service.Services.Abstractions
{
    public interface IUserService
    {
        Task<ApiResponse> ChargeCard();
        Task<ApiResponse> TopUpBalance(string paymentMethodId, string userId, double amount, string currency = "azn");
        Task<ApiResponse> UpdatePfp(UpdatePfpDto dto);
    }
}
