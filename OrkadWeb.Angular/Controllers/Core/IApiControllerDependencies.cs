using MediatR;

namespace OrkadWeb.Angular.Controllers.Core
{
    /// <summary>
    /// Group of <see cref="ApiController"/> dependencies.
    /// Main purpose is you don't need to change every ctor when a dependency is added or removed
    /// </summary>
    public interface IApiControllerDependencies
    {
        ISender Sender { get; }
        IPublisher Publisher { get; }
    }
}
