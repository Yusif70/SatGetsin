using Microsoft.Extensions.Configuration;
using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Services.Abstractions;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace SatGetsin2.Service.Services.Concretes
{
	public class SmsService : ISmsService
	{
		private readonly IConfiguration _config;

		public SmsService(IConfiguration config)
		{
			_config = config;
		}
		public async Task<ApiResponse> SendSms(string to, string body)
		{
			//string from = _config["Vonage:From"];
			//string apiKey = _config["Vonage:ApiKey"];
			//string apiSecret = _config["Vonage:ApiSecret"];
			string from = "+994559027661";
			string apiKey = "fefabc94";
			string apiSecret = "Ei155l87gwoTzdBZ";
			var credentials = Credentials.FromApiKeyAndSecret(apiKey, apiSecret);
			VonageClient client = new(credentials);
			var res = await client.SmsClient.SendAnSmsAsync(new SendSmsRequest()
			{
				To = to,
				From = from,
				Text = body
			});
			if (res != null && Convert.ToInt32(res.MessageCount) > 0 && res.Messages[0].StatusCode.ToString() == "Success")
			{
				return new ApiResponse { StatusCode = 200, Message = "Verification code sent to your phone number" };
			}
			else
			{
				return new ApiResponse { StatusCode = 500, Message = "Something went wrong" };
			}
		}
	}
}
