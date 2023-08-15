﻿using Microsoft.Extensions.Logging;

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


    [System.Serializable]
    public class EmailConfirmationException : System.Exception
    {
        public EmailConfirmationException() { }
        public EmailConfirmationException(string message) : base(message) { }
        public EmailConfirmationException(string message, System.Exception inner) : base(message, inner) { }
        protected EmailConfirmationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

internal static partial class LoggerMessageDefinitions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "email {Email} of user {Username} is validated")]
    internal static partial void LogEmailValidationSucess(this ILogger logger, string email, string username);

    [LoggerMessage(Level = LogLevel.Warning, Message = "email {Email} of user {Username} failed: {Reason}")]
    internal static partial void LogEmailValidationFail(this ILogger logger, string email, string username, string reason);
}