using MediatR;

namespace OrkadWeb.Domain.Common
{
    /// <summary>
    /// Handle the specified command. Assuming the command is in validated state
    /// </summary>
    /// <typeparam name="TCommand">Type of the command</typeparam>
    /// <typeparam name="TResponse">Type of the command response</typeparam>
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse> { }

    /// <summary>
    /// Handle the specfied command (without response). Assuming the command is in validated state
    /// </summary>
    /// <typeparam name="TCommand">Type of the responseless command</typeparam>
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand> where TCommand : ICommand { }
}
