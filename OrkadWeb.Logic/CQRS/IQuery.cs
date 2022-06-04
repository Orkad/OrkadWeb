using MediatR;

namespace OrkadWeb.Logic.CQRS
{
    /// <summary>
    /// Marker interface to represent a query with a response.
    /// </summary>
    /// <typeparam name="TResponse">Type de réponse de la requête</typeparam>
    public interface IQuery<out TResponse> : IRequest<TResponse> { }

    /// <summary>
    /// Représente une requête ne fournissant pas de réponse selon une architecture CQRS
    /// </summary>
    public interface IQuery : IQuery<Unit> { }
}
