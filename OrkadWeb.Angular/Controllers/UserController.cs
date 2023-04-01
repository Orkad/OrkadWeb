using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Angular.Controllers.Core;
using OrkadWeb.Application.Users.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    [Route("api/users")]
    public class UserController : ApiController
    {
        public UserController(IApiControllerDependencies deps) : base(deps)
        {
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public async Task<List<GetAllUsersQuery.Result>> GetAll() => await Query(new GetAllUsersQuery());
    }
}
