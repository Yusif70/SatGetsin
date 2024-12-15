using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.Auth;

namespace SatGetsin2.Service.Services.Abstractions
{
    public interface IAuthService
    {
        Task<ApiResponse> CreateRole();
        Task<ApiResponse> Register(RegisterDto dto);
        Task<ApiResponse> SendConfirmationEmail(string userId);
        Task<ApiResponse> VerifyEmail(string userId, string token, int input);
        Task<ApiResponse> SendConfirmationSms(string userId);
        Task<ApiResponse> VerifyPhone(string userId, string token, int input);
        Task<ApiResponse> Login(LoginDto dto);
        ApiResponse SendResetPasswordEmail(string email);
        ApiResponse ConfirmCode(string email, int code);
        Task<ApiResponse> ResetPassword(string email, ResetPasswordDto dto);
        void Logout();
    }
}
