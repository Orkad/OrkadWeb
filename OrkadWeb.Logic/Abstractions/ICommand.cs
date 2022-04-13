using MediatR;

namespace OrkadWeb.Logic
{
    /// <summary>
    /// Représente une commande fournissant une réponse selon une architecture CQRS
    /// </summary>
    /// <typeparam name="TResponse">Type de réponse de la commande</typeparam>
    public interface ICommand<out TResponse> : IRequest<TResponse> { }

    /// <summary>
    /// Représente une commande ne fournissant pas de réponse selon une architecture CQRS
    /// </summary>
    public interface ICommand : ICommand<Unit> { }
}
