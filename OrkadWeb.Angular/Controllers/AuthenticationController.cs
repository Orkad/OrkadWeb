using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.Users.Commands;

namespace OrkadWeb.Angular.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public AuthenticationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //[HttpGet("context")]
        //public LoginResult Context() => GetAuthenticatedUser();

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<LoginResponse> Login(LoginCommand command)
        {
            var response = await mediator.Send(command, HttpContext.RequestAborted);
            if (response.Success)
            {
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.PrimarySid, response.Id.ToString()),
                    new Claim(ClaimTypes.Name, response.Name),
                    new Claim(ClaimTypes.Email, response.Email),
                    new Claim(ClaimTypes.Role, response.Role),
                };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                    IsPersistent = true,
                };
                await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, principal, authProperties);
            }
            return response;
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

        //private LoginResult GetAuthenticatedUser()
        //{
        //    return new LoginResult
        //    {
        //        Id = User.FindFirstValue(ClaimTypes.PrimarySid),
        //        Name = User.Identity.Name,
        //        Email = User.FindFirstValue(ClaimTypes.Email),
        //        Role = User.FindFirstValue(ClaimTypes.Role),
        //        Error = null,
        //    };
        //}
    }
}