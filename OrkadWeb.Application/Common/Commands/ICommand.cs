using MediatR;

namespace OrkadWeb.Domain.Common
{
    /// <summary>
    /// Représente une commande fournissant une réponse selon une architecture CQRS
    /// </summary>
    /// <typeparam name="TResponse">Command response type.</typeparam>
    public interface ICommand<out TResponse> : IRequest<TResponse> { }

    /// <summary>
    /// Représente une commande ne fournissant pas de réponse selon une architecture CQRS
    /// </summary>
    public interface ICommand : IRequest { }
}
