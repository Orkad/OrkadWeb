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
using OrkadWeb.Services.Authentication;
using OrkadWeb.Services.Data;

namespace OrkadWebVue.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly LoginService loginService;

        public AccountController(LoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpGet("context")]
        public LoginResult Context() => GetAuthenticatedUser();

        [HttpPost("login")]
        public async Task<LoginResult> Login([FromBody]LoginCredentials loginCredentials)
        {
            var result = loginService.Login(loginCredentials);
            if (result.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.PrimarySid, result.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.Name),
                    new Claim(ClaimTypes.Email, result.Email),
                    new Claim(ClaimTypes.Role, result.Role),
                };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                    IsPersistent = true,
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            }
            return result;
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

        private LoginResult GetAuthenticatedUser()
        {
            return new LoginResult
            {
                Id = User.FindFirstValue(ClaimTypes.PrimarySid),
                Name = User.Identity.Name,
                Email = User.FindFirstValue(ClaimTypes.Email),
                Role = User.FindFirstValue(ClaimTypes.Role),
                Error = null,
            };
        }
    }
}