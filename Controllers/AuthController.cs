using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MVCAPI.Controllers
{
    //[Produces("application/json")]
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
          IConfiguration config,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider = "Facebook", string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                //return RedirectToAction(nameof(Login));
                return BadRequest();
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                // return RedirectToAction(nameof(Login));
                return BadRequest();
            }
            var a1 = info.ProviderKey;
            var a2 = info.Principal;
            var a6 = a2.Claims;

            //if (result.Succeeded)
            //{

            //    var claims = new[]
            //    {
            //  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            //  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //};

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789ABCDEF"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken("http://mycodecamp.io",
                  "http://mycodecamp.io",
                  a6,
                  expires: DateTime.Now.AddDays(30),
                  signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            

            // Sign in the user with this external login provider if the user already has a login.
            //var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            //if (result.Succeeded)
            //{
            //    _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
            //    return RedirectToLocal(returnUrl);
            //}
            //if (result.IsLockedOut)
            //{
            //    return RedirectToAction(nameof(Lockout));
            //}
            //else
            //{
            //    // If the user does not have an account, then ask the user to create an account.
            //    ViewData["ReturnUrl"] = returnUrl;
            //    ViewData["LoginProvider"] = info.LoginProvider;
            //    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            //    return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            //}
           
        }


    }
}