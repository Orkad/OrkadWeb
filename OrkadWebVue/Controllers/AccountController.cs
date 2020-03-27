using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Models;
using OrkadWeb.Services;

namespace OrkadWebVue.Controllers
{
    public class LoginCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public string Error { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IDataService dataService;

        public AccountController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet("context")]
        public IActionResult Context()
        {
            return Json(new
            {
                name = this.User?.Identity?.Name,
                email = this.User?.FindFirstValue(ClaimTypes.Email),
                role = this.User?.FindFirstValue(ClaimTypes.Role),
            });
        }

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
                    Error = "La combinaison renseignée est incorrecte, veuillez réésayer"
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
            var hash = HashUtils.HashSHA256(creds.Password);
            return dataService.Query<User>().Any(u => (u.Username == creds.Username || u.Email == creds.Username) && u.Password == hash);
        }

        // On a real project, you would use the SignInManager 
        // to locate the user by its email and build its ClaimsPrincipal:
        //  var user = await _signInManager.UserManager.FindByEmailAsync(email);
        //  var principal = await _signInManager.CreateUserPrincipalAsync(user)
        private ClaimsPrincipal GetPrincipal(LoginCredentials creds, string authScheme)
        {
            var hash = HashUtils.HashSHA256(creds.Password);
            var user = dataService.Query<User>().SingleOrDefault(u => (u.Username == creds.Username || u.Email == creds.Username) && u.Password == hash);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User"),
            };
            return new ClaimsPrincipal(new ClaimsIdentity(claims, authScheme));
        }
    }
}