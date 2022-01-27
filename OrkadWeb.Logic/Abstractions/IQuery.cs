using MediatR;

namespace OrkadWeb.Logic
{
    /// <summary>
    /// Représente une requête fournissant une réponse selon une architecture CQRS
    /// </summary>
    /// <typeparam name="TResponse">Type de réponse de la requête</typeparam>
    interface IQuery<out TResponse> : IRequest<TResponse> { }

    /// <summary>
    /// Représente une requête ne fournissant pas de réponse selon une architecture CQRS
    /// </summary>
    interface IQuery : IRequest { }
}
