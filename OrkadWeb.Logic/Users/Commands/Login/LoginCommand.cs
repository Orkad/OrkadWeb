using MediatR;
using System;
using System.Collections.Generic;

namespace OrkadWeb.Logic.Users.Commands
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
