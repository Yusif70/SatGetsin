using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SatGetsin2.Core.Entities;
using SatGetsin2.Data.Context;
using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.Auth;
using SatGetsin2.Service.Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SatGetsin2.Service.Services.Concretes
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ISmsService _smsService;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        private static readonly Dictionary<string, (int Code, DateTime Expiry)> verificationCodes = new();
        private static readonly Dictionary<string, (int Code, DateTime Expiry)> resetCodes = new();
        public AuthService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, IMailService mailService, ISmsService smsService, AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _mailService = mailService;
            _smsService = smsService;
            _context = context;
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
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var user = new AppUser
                {
                    UserName = dto.UserName,
                    FullName = dto.FullName,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                var res = await _userManager.CreateAsync(user, dto.Password);
                if (!res.Succeeded)
                {
                    return new ApiResponse { StatusCode = 500, Data = res.Errors.Select(e => e.Description) };
                }
                var res1 = await _userManager.AddToRoleAsync(user, "User");
                if (!res1.Succeeded)
                {
                    return new ApiResponse { StatusCode = 500, Data = res1.Errors.Select(e => e.Description) };
                }
                var res2 = await SendConfirmationEmail(user.Id);
                //var res2 = await SendConfirmationSms(user.Id);
                await transaction.CommitAsync();
                return new ApiResponse { StatusCode = 200, Data = res2.Data };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponse { StatusCode = 500, Data = ex.Message };
            }
        }
        public async Task<ApiResponse> SendConfirmationEmail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            Random random = new();
            int code = random.Next(1000, 9999);
            verificationCodes[user.Id] = (code, DateTime.Now.AddMinutes(2));
            _mailService.SendMail(user.Email, "Verify email", $"Please verify your email address using the code below to complete registration\n{code}");
            return new ApiResponse { StatusCode = 200, Data = new { UserId = user.Id, Token = token } };
        }
        public async Task<ApiResponse> VerifyEmail(string userId, string token, int code)
        {
            if (verificationCodes.TryGetValue(userId, out var verification))
            {
                if (verification.Expiry < DateTime.Now || verification.Code != code)
                {
                    return new ApiResponse { StatusCode = 400, Message = "Code not valid" };
                }
                verificationCodes.Remove(userId);
                var user = await _userManager.FindByIdAsync(userId);
                var res = await _userManager.ConfirmEmailAsync(user, token);
                if (!res.Succeeded)
                {
                    return new ApiResponse { StatusCode = 500, Data = res.Errors.Select(e => e.Description) };
                }
                await _signInManager.SignInAsync(user, isPersistent: true);
                return new ApiResponse { StatusCode = 200, Message = "Verification successful" };
            }
            return new ApiResponse { StatusCode = 404 };
        }
        public async Task<ApiResponse> SendConfirmationSms(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            Random random = new();
            int code = random.Next(1000, 9999);
            verificationCodes[user.Id] = (code, DateTime.Now.AddMinutes(2));
            var res = await _smsService.SendSms(user.PhoneNumber, $"Your verification code {code}, valid for 2 minutes");
            if (res.StatusCode == 200)
            {
                return new ApiResponse { StatusCode = 200, Data = new { UserId = user.Id, Token = token } };
            }
            else
            {
                return new ApiResponse { StatusCode = 500, Message = res.Message };
            }
        }
        public async Task<ApiResponse> VerifyPhone(string userId, string token, int code)
        {
            if (verificationCodes.TryGetValue(userId, out var verification))
            {
                if (verification.Expiry < DateTime.Now || verification.Code != code)
                {
                    return new ApiResponse { StatusCode = 400, Message = "Code not valid" };
                }
                verificationCodes.Remove(userId);
                var user = await _userManager.FindByIdAsync(userId);
                var res = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);
                if (!res.Succeeded)
                {
                    return new ApiResponse { StatusCode = 500, Data = res.Errors.Select(e => e.Description) };
                }
                await _signInManager.SignInAsync(user, isPersistent: true);
                return new ApiResponse { StatusCode = 200, Message = "Verification successful" };
            }
            return new ApiResponse { StatusCode = 404 };
        }
        public async Task<ApiResponse> Login(LoginDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser == null)
            {
                return new ApiResponse { StatusCode = 404, Message = "Username or password not valid" };
            }
            var loggedIn = await _signInManager.PasswordSignInAsync(existingUser, dto.Password, dto.RememberMe, true);
            if (!loggedIn.Succeeded)
            {
                return new ApiResponse { StatusCode = 404, Message = "Username or password not valid" };
            }
            var canSignIn = await _signInManager.CanSignInAsync(existingUser);
            if (!canSignIn)
            {
                return new ApiResponse { StatusCode = 404 };
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
                expires: DateTime.Now.AddDays(Convert.ToDouble(_config["JWT:datetime"])),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new ApiResponse { StatusCode = 200, Data = jwtToken };
        }
        public ApiResponse SendResetPasswordEmail(string email)
        {
            Random random = new();
            int code = random.Next(1000, 9999);
            resetCodes[email] = (code, DateTime.Now.AddMinutes(2));
            _mailService.SendMail(email, "Reset password", $"Please reset your password using the code below\n{code}");
            return new ApiResponse { StatusCode = 200, Data = new { Email = email } };
        }
        public ApiResponse ConfirmCode(string email, int code)
        {
            if (resetCodes.TryGetValue(email, out var reset))
            {
                if (reset.Expiry < DateTime.Now || reset.Code != code)
                {
                    return new ApiResponse { StatusCode = 400, Message = "Code not valid" };
                }
                resetCodes.Remove(email);
                return new ApiResponse { StatusCode = 200, Data = new { Email = email } };
            }
            return new ApiResponse { StatusCode = 404, Message = "No account associated with this account found" };
        }
        public async Task<ApiResponse> ResetPassword(string email, [FromForm] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse { StatusCode = 404 };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var res = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
            if (!res.Succeeded)
            {
                return new ApiResponse { StatusCode = 500, Data = res.Errors.Select(e => e.Description) };
            }
            return new ApiResponse { StatusCode = 200, Message = "Password reset successfully!" };
        }
        public async void Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}