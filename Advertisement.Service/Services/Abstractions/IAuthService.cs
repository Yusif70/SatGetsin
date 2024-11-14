using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Advertisement.Service.Services.Abstractions
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
        Task<ApiResponse> ResetPassword(string email, [FromForm] ResetPasswordDto dto);
        void Logout();
    }
}
