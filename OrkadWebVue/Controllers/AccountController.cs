using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrkadWebVue.Controllers
{
    public class LoginCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public string Error { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginCredentials loginCredentials)
        {
            // We will typically move the validation of credentials
            // and return of matched principal into its own AuthenticationService
            // Leaving it here for convenience of the sample project/article
            if (!ValidateLogin(loginCredentials))
            {
                return Ok(new LoginResult
                {
                    Error = "Login failed"
                });
            }
            var principal = GetPrincipal(loginCredentials, Startup.COOKIE_AUTH_SCHEME);
            await HttpContext.SignInAsync(Startup.COOKIE_AUTH_SCHEME, principal);

            return Ok(new LoginResult
            {
                Name = principal.Identity.Name,
                Email = principal.FindFirstValue(ClaimTypes.Email),
                Role = principal.FindFirstValue(ClaimTypes.Role)
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        // On a real project, you would use a SignInManager to verify the identity
        // using:
        //  _signInManager.PasswordSignInAsync(user, password, lockoutOnFailure: false);
        // With JWT you would rather avoid that to prevent cookies being set and use: 
        //  _signInManager.UserManager.FindByEmailAsync(email);
        //  _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        private bool ValidateLogin(LoginCredentials creds)
        {
            // For our sample app, all logins are successful!
            return true;
        }

        // On a real project, you would use the SignInManager 
        // to locate the user by its email and build its ClaimsPrincipal:
        //  var user = await _signInManager.UserManager.FindByEmailAsync(email);
        //  var principal = await _signInManager.CreateUserPrincipalAsync(user)
        private ClaimsPrincipal GetPrincipal(LoginCredentials creds, string authScheme)
        {
            // Here we are just creating a Principal for any user, 
            // using its email and a hardcoded “User” role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, creds.Email),
                new Claim(ClaimTypes.Email, creds.Email),
                new Claim(ClaimTypes.Role, "User"),
            };
            return new ClaimsPrincipal(new ClaimsIdentity(claims, authScheme));
        }
    }
}