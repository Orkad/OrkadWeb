using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Logic.CQRS;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers
{
    /// <summary>
    /// Classe de base dont tous les controlleurs de l'api doivent hériter
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public abstract class ApiController : ControllerBase
    {
        private readonly IMediator mediator;

        protected ApiController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Run a CQRS query
        /// </summary>
        public async Task<T> Query<T>(IQuery<T> query) => await mediator.Send(query, HttpContext.RequestAborted);

        /// <summary>
        /// Run a CQRS command without result
        /// </summary>
        public async Task Command(ICommand command) => await mediator.Send(command, HttpContext.RequestAborted);

        /// <summary>
        /// Run a CQRS command
        /// </summary>
        public async Task<T> Command<T>(ICommand<T> command) => await mediator.Send(command, HttpContext.RequestAborted);
    }
}
