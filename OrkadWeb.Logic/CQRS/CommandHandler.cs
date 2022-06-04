using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.CQRS
{
    /// <summary>
    /// Base class for handling commands.
    /// </summary>
    /// <typeparam name="TCommand">Command type to handle.</typeparam>
    /// <typeparam name="TResponse">Response of command type to handle.</typeparam>
    public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        /// <inheritdoc/>
        public Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken) => HandleCommand(command, cancellationToken);

        /// <summary>
        /// Handles a command.
        /// </summary>
        /// <param name="command">Command to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response from the command.</returns>
        protected abstract Task<TResponse> HandleCommand(TCommand command, CancellationToken cancellationToken);
    }

    /// <inheritdoc/>
    public abstract class CommandHandler<TCommand> : CommandHandler<TCommand, Unit> where TCommand : ICommand
    {

    }
}
