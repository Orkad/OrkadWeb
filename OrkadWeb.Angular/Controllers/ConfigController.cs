using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.Config.Queries;

namespace OrkadWeb.Angular.Controllers;

[ApiController]
[Authorize]
[Route("api/config/[action]")]
public class ConfigController : ControllerBase
{
    private readonly ISender sender;

    public ConfigController(ISender sender)
    {
        this.sender = sender;
    }

    /// <summary>
    ///     Access to the global configuration
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<GetGlobalConfigurationQuery.Result> Global(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetGlobalConfigurationQuery(), cancellationToken);
    }
}