﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.Users.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    [Route("api/users")]
    public class UserController : ApiController
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public async Task<List<GetAllUsersQuery.Result>> GetAll() => await Query(new GetAllUsersQuery());
    }
}
