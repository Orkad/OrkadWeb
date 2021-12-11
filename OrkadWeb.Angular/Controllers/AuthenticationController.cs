using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrkadWeb.Logic.Users.Commands;

namespace OrkadWeb.Angular.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public AuthenticationController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<LoginResponse> Login(LoginCommand command)
        {
            var response = await mediator.Send(command, HttpContext.RequestAborted);
            if (response.Success)
            {
                response.Token = GenerateJSONWebToken(response);  
            }
            return response;
        }

        private string GenerateJSONWebToken(LoginResponse loginResponse)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ConvertToUnixTimestamp(DateTime.Now).ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, loginResponse.Name),
                new Claim(JwtRegisteredClaimNames.Email, loginResponse.Email),
            };
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
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