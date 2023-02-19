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
using OrkadWeb.Application.Abstractions;
using OrkadWeb.Application.Users;
using OrkadWeb.Application.Users.Commands;
using OrkadWeb.Application.Users.Notifications;

namespace OrkadWeb.Angular.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<LoginCommand.Result> Login(LoginCommand command)
        {
            var result = await Command(command);
            await Publish(new UserLoggedInNotification
            {
                UserName = command.Username,
            });
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Register(RegisterCommand command)
        {
            await Command(command);
            await Publish(new UserRegisteredNotification
            {
                UserName = command.UserName,
            });
        }
    }
}