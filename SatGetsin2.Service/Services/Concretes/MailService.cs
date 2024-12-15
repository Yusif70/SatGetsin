using SatGetsin2.Service.Services.Abstractions;
using Microsoft.Extensions.Configuration;
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

        public async void SendMail(string to, string subject, string body)
        {
            string from = _config["Mail:From"];
            MailMessage mail = new()
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(from),
            };
            mail.To.Add(to);
            SmtpClient smtp = new()
            {
                Host = _config["Smtp:Host"],
                Port = Convert.ToInt32(_config["Smtp:Port"]),
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, _config["Smtp:Password"]),
                EnableSsl = true
            };
            smtp.Send(mail);
        }
    }
}
