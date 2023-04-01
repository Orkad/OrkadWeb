using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Angular.Controllers.Core;
using OrkadWeb.Application.Config.Queries;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    public class ConfigController : ApiController
    {
        public ConfigController(IApiControllerDependencies deps) : base(deps)
        {
        }

        /// <summary>
        /// Access to the global configuration
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<GetGlobalConfigurationQuery.Result> Global() => await Query(new GetGlobalConfigurationQuery());
    }
}
