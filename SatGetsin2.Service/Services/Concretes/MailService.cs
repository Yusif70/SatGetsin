using Microsoft.Extensions.Configuration;
using SatGetsin2.Service.Services.Abstractions;
using System.Net;
using System.Net.Mail;

namespace SatGetsin2.Service.Services.Concretes
{
	public class MailService : IMailService
	{
		private readonly IConfiguration _config;

		public MailService(IConfiguration config)
		{
			_config = config;
		}

		public void SendMail(string to, string subject, string body)
		{
			//string from = _config["Mail:From"];
			string from = "yusifpirquliyev7@gmail.com";
			MailMessage mail = new()
			{
				Subject = subject,
				Body = body,
				From = new MailAddress(from),
			};
			mail.To.Add(to);
			SmtpClient smtp = new()
			{
				//Host = _config["Smtp:Host"],
				//Port = Convert.ToInt32(_config["Smtp:Port"]),
				Host = "smtp.gmail.com",
				Port = 587,
				UseDefaultCredentials = false,
				//Credentials = new NetworkCredential(from, _config["Smtp:Password"]),
				Credentials = new NetworkCredential(from, "bzft xzii hzzk lvpr"),
				EnableSsl = true
			};
			smtp.Send(mail);
		}
	}
}
