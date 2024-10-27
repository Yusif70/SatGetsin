using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.Auth;
using Microsoft.AspNetCore.Identity;

namespace Advertisement.Service.Services.Abstractions
{
    public interface IAuthService
    {
        Task<ApiResponse> CreateRole();
        Task<ApiResponse> Register(RegisterDto dto);
        Task<ApiResponse> SendEmail(IdentityUser user);
        Task<ApiResponse> VerifyEmail(IdentityUser user, string token, int input);
        Task<ApiResponse> Login(LoginDto dto);
        void Logout();
    }
}
