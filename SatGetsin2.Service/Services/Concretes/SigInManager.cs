using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SatGetsin2.Service.Services.Concretes
{
    public class CustomSignInManager<T> : SignInManager<T> where T : IdentityUser
    {
        public CustomSignInManager(UserManager<T> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<T> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<T>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<T> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) { }

        public override async Task<bool> CanSignInAsync(T user)
        {
            var emailConfirmed = await UserManager.IsEmailConfirmedAsync(user);
            var phoneConfirmed = await UserManager.IsPhoneNumberConfirmedAsync(user);
            return emailConfirmed || phoneConfirmed;
        }
    }
}
