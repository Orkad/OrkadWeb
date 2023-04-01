using MediatR;

namespace OrkadWeb.Angular.Controllers.Core;

public class ApiControllerDependencies : IApiControllerDependencies
{
    public ApiControllerDependencies(ISender sender, IPublisher publisher)
    {
        Sender = sender;
        Publisher = publisher;
    }

    public ISender Sender { get; }

    public IPublisher Publisher { get; }
}