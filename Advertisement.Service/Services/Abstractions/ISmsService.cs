using Advertisement.Service.ApiResponses;

namespace Advertisement.Service.Services.Abstractions
{
    public interface ISmsService
    {
        Task<ApiResponse> SendSms(string to, string body);
    }
}
