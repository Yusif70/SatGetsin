using Advertisement.Service.ApiResponses;
using Advertisement.Service.Dtos.Auth;
using Advertisement.Service.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Advertisement.Service.Services.Concretes
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;

        private static readonly Dictionary<string, (int Code, DateTime Expiry)> verificationCodes = [];
        public AuthService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config, IMailService mailService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _mailService = mailService;
        }

        public async Task<ApiResponse> CreateRole()
        {
            var role1 = new IdentityRole
            {
                Name = "Admin"
            };
            var role2 = new IdentityRole
            {
                Name = "SuperAdmin"
            };
            var role3 = new IdentityRole
            {
                Name = "User"
            };
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role3);
            return new ApiResponse { StatusCode = 201, Message = "rollar yarandi" };
        }

        public async Task<ApiResponse> Register([FromForm] RegisterDto dto)
        {
            var user = new IdentityUser
            {
                UserName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var res = await _userManager.CreateAsync(user, dto.Password);
            if (!res.Succeeded)
            {
                return new ApiResponse { StatusCode = 500, Message = res.Errors.ToString() };
            }
            var res1 = await _userManager.AddToRoleAsync(user, "User");
            if (!res1.Succeeded)
            {
                return new ApiResponse { StatusCode = 500, Message = res1.Errors.ToString() };
            }

            var res2 = await SendEmail(user);
            return new ApiResponse { StatusCode = 200, Message = "Verification code was sent to your email", Data = res2.Data };
        }
        public async Task<ApiResponse> SendEmail(IdentityUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            Random random = new();
            int code = random.Next(1000, 9999);
            verificationCodes[user.Id] = (code, DateTime.Now.AddMinutes(2));
            _mailService.SendMail(user.Email, "Verify mail", $"Please verify your email address using the code below to complete registration\n{code}");
            //VerifyEmail(user, input);
            return new ApiResponse { StatusCode = 200, Data = (user, token) };
        }
        public async Task<ApiResponse> VerifyEmail(IdentityUser user, string token, int input)
        {
            if (verificationCodes.TryGetValue(user.Id, out var verification))
            {
                if (verification.Expiry < DateTime.Now || verification.Code != input)
                {
                    return new ApiResponse { StatusCode = 400, Message = "Code not valid" };
                }
                verificationCodes.Remove(user.Id);
                await _userManager.ConfirmEmailAsync(user, token);
                await _signInManager.SignInAsync(user, isPersistent: true);
                return new ApiResponse { StatusCode = 200, Message = "Verification successfull" };
            }
            return new ApiResponse { StatusCode = 404 };
        }
        public async Task<ApiResponse> Login(LoginDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Username or password is invalid" };
            }
            var loggedIn = await _userManager.CheckPasswordAsync(existingUser, dto.Password);
            if (!loggedIn)
            {
                return new ApiResponse { StatusCode = 404, Message = "Username or password is invalid" };
            }
            var userRoles = await _userManager.GetRolesAsync(existingUser);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, existingUser.UserName),
                new(ClaimTypes.NameIdentifier, existingUser.Id)
            };
            foreach (var role in userRoles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:secret_key"]));

            var token = new JwtSecurityToken(
                issuer: _config["JWT:issuer"],
                audience: _config["JWT:audience"],
                expires: DateTime.Now.AddHours(Convert.ToDouble(_config["JWT:datetime"])),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new ApiResponse { StatusCode = 200, Data = jwtToken };
        }
        public async void Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}