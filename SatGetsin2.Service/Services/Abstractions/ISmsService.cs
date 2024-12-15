using SatGetsin2.Service.ApiResponses;

namespace SatGetsin2.Service.Services.Abstractions
{
    public interface ISmsService
    {
        Task<ApiResponse> SendSms(string to, string body);
    }
}
