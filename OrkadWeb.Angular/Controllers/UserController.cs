﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.Users.Commands;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    public class UserController : ApiController
    {
        public UserController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [AllowAnonymous]
        public async Task Register(RegisterCommand command) => await Command(command);
    }
}
