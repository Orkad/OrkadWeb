using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.Config.Queries;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    public class ConfigController : ApiController
    {
        private readonly IMediator mediator;

        public ConfigController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Access to the global configuration
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<GlobalConfigurationResult> Global() => await mediator.Send(new GetGlobalConfigurationQuery());
    }
}
