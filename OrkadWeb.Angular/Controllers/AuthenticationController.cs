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
using OrkadWeb.Angular.Config;
using OrkadWeb.Logic.Abstractions;
using OrkadWeb.Logic.Users.Commands;

namespace OrkadWeb.Angular.Controllers
{
    public class AuthenticationController : ApiController
    {
        private readonly IConfiguration configuration;
        private readonly ITimeProvider timeProvider;
        private readonly JwtConfig jwtConfig;

        public AuthenticationController(IMediator mediator, IConfiguration configuration, ITimeProvider timeProvider, JwtConfig jwtConfig)
            : base(mediator)
        {
            this.configuration = configuration;
            this.timeProvider = timeProvider;
            this.jwtConfig = jwtConfig;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<LoginCommand.Result> Login(LoginCommand command)
        {
            var response = await Command(command);
            if (response.Success)
            {
                response.Token = jwtConfig.GenerateToken(GetClaims(response));
            }
            return response;
        }

        private Claim[] GetClaims(LoginCommand.Result loginResponse)
        {
            return new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ConvertToUnixTimestamp(DateTime.Now).ToString()),
                new Claim("user_id", loginResponse.Id),
                new Claim("user_name", loginResponse.Name),
                new Claim("user_email", loginResponse.Email),
            };
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}