using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

    public class AuthenticatedUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IDataService dataService;

        public AccountController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet("context")]
        public dynamic Context()
        {
            return new
            {
                Id = this.User?.FindFirstValue(ClaimTypes.PrimarySid),
                Name = this.User?.Identity?.Name,
                Email = this.User?.FindFirstValue(ClaimTypes.Email),
                Role = this.User?.FindFirstValue(ClaimTypes.Role),
            };
        }

        [HttpPost("login")]
        public async Task<dynamic> Login([FromBody]LoginCredentials loginCredentials)
        {
            if (!ValidateLogin(loginCredentials))
            {
                return new
                {
                    Error = "La combinaison renseignée est incorrecte, veuillez réésayer"
                };
            }
            var hash = HashUtils.HashSHA256(loginCredentials.Password);
            var user = dataService.Query<User>().SingleOrDefault(u => (u.Username == loginCredentials.Username || u.Email == loginCredentials.Username) && u.Password == hash);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User"),
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            return new
            {
                Id = principal.FindFirstValue(ClaimTypes.PrimarySid),
                Name = principal.Identity.Name,
                Email = principal.FindFirstValue(ClaimTypes.Email),
                Role = principal.FindFirstValue(ClaimTypes.Role)
            };
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

        private bool ValidateLogin(LoginCredentials creds)
        {
            var hash = HashUtils.HashSHA256(creds.Password);
            return dataService.Query<User>().Any(u => (u.Username == creds.Username || u.Email == creds.Username) && u.Password == hash);
        }
    }
}