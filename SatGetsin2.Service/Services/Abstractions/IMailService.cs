namespace SatGetsin2.Service.Services.Abstractions
{
    public interface IMailService
    {
        void SendMail(string to, string subject, string body);
    }
}
