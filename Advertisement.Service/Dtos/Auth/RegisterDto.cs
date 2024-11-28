using Microsoft.AspNetCore.Http;

namespace Advertisement.Service.Dtos.Auth
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public IFormFile File { get; set; }
    }
}
