namespace OrkadWeb.Domain.Common;

/// <summary>
/// Marker interface to represent a query with a response.
/// </summary>
/// <typeparam name="TResponse">Type de réponse de la requête</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse> { }