using Microsoft.Extensions.Logging;
using System;

namespace OrkadWeb.Application.Users.Commands;

public class EmailConfirmCommand : ICommand
{
    /// <summary>
    /// Email to confirm
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// Confirmation hash
    /// </summary>
    public string Hash { get; init; }

    class Handler : ICommandHandler<EmailConfirmCommand>
    {
        private readonly IDataService dataService;
        private readonly ITimeProvider timeProvider;
        private readonly ILogger<Handler> logger;

        public Handler(IDataService dataService, ITimeProvider timeProvider, ILogger<Handler> logger)
        {
            this.dataService = dataService;
            this.timeProvider = timeProvider;
            this.logger = logger;
        }

        public async Task Handle(EmailConfirmCommand command, CancellationToken cancellationToken)
        {
            using (var context = dataService.Context())
            {
                var user = await dataService.FindAsync<User>(u => u.Email == command.Email, cancellationToken);
                if (user == null)
                {
                    throw new EmailConfirmationException("user was not found");
                }
                if (user.Confirmation != null)
                {
                    var reason = "user already confirmed email";
                    logger.LogEmailValidationFail(user.Email, user.Username, reason);
                    throw new EmailConfirmationException(reason);
                }
                if (!Security.Hash.Validate(user.Email, command.Hash))
                {
                    var reason = "wrong confirmation hash";
                    logger.LogEmailValidationFail(user.Email, user.Username, reason);
                    throw new EmailConfirmationException(reason);
                }

                user.Confirmation = timeProvider.Now;
                await context.SaveChanges(cancellationToken);
                logger.LogEmailValidationSucess(user.Email, user.Username);
            }
        }
    }

    internal sealed class EmailConfirmationException(string message) : Exception(message)
    {
    }
}