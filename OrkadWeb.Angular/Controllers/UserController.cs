using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.Users.Queries;

namespace OrkadWeb.Angular.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ISender sender;

    public UserController(ISender sender)
    {
        this.sender = sender;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("")]
    public async Task<List<GetAllUsersQuery.Result>> GetAll(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetAllUsersQuery(), cancellationToken);
    }
}