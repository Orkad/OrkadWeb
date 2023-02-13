using MediatR;

namespace OrkadWeb.Domain.Common
{
    /// <summary>
    /// Handle the specified command. Assuming the command is in validated state
    /// </summary>
    /// <typeparam name="TQuery">Type of the command</typeparam>
    /// <typeparam name="TResponse">Type of the command response</typeparam>
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse> { }

    /// <summary>
    /// Handle the specfied command (without response). Assuming the command is in validated state
    /// </summary>
    /// <typeparam name="TCommand">Type of the responseless command</typeparam>
    public interface IQueryHandler<TQuery> : IRequestHandler<TQuery> where TQuery : ICommand { }
}
