using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Domain.Common;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Controllers.Core
{
    /// <summary>
    /// Classe de base dont tous les controlleurs de l'api doivent hériter : api/[controller]/[action]
    /// </summary>
    [ApiController, Authorize, Route("api/[controller]/[action]")]
    public abstract class ApiController : ControllerBase
    {
        private readonly IApiControllerDependencies deps;

        protected ApiController(IApiControllerDependencies deps)
        {
            this.deps = deps;
        }

        /// <summary>
        /// Run a CQRS query
        /// </summary>
        public async Task<T> Query<T>(IQuery<T> query) => await deps.Sender.Send(query, HttpContext.RequestAborted);

        /// <summary>
        /// Run a CQRS command without result
        /// </summary>
        public async Task Command(ICommand command) => await deps.Sender.Send(command, HttpContext.RequestAborted);

        /// <summary>
        /// Run a CQRS command
        /// </summary>
        public async Task<T> Command<T>(ICommand<T> command) => await deps.Sender.Send(command, HttpContext.RequestAborted);

        /// <summary>
        /// Publish a notification
        /// </summary>
        public async Task Publish(INotification notification) => await deps.Publisher.Publish(notification, HttpContext.RequestAborted);
    }


}
