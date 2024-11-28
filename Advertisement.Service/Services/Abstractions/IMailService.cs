namespace Advertisement.Service.Services.Abstractions
{
    public interface IMailService
    {
        void SendMail(string to, string subject, string body);
    }
}
