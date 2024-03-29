﻿using Microsoft.Extensions.Logging;
using OrkadWeb.Application.Security;
using OrkadWeb.Application.Users.Notifications;
using OrkadWeb.Domain.Consts;
using System;
using GlobalConfiguration = OrkadWeb.Application.Config.GlobalConfiguration;

namespace OrkadWeb.Application.Users.Commands;

public class RegisterCommand : ICommand
{
    /// <summary>
    /// (required) username 5 to 32 characters
    /// </summary>
    public string UserName { get; init; }

    /// <summary>
    /// (required) valid email adress
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// (required) password with at least 8 characters, one lower, one upper, and one special character
    /// </summary>
    public string Password { get; init; }

    public class Validator : AbstractValidator<RegisterCommand>
    {
        private readonly IDataService dataService;

        public Validator(IDataService dataService)
        {
            this.dataService = dataService;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(command => command.UserName)
                .NotEmpty().WithErrorCode(ErrorCodes.USERNAME_REQUIRED)
                .MinimumLength(GlobalConfiguration.USERNAME_MIN_LENGHT).WithErrorCode(ErrorCodes.USERNAME_TOO_SHORT)
                .MaximumLength(GlobalConfiguration.USERNAME_MAX_LENGHT).WithErrorCode(ErrorCodes.USERNAME_TOO_LONG)
                .Matches(GlobalConfiguration.USERNAME_REGEX).WithErrorCode(ErrorCodes.USERNAME_WRONG_FORMAT)
                .Must(NotMatchAnotherUsername).WithErrorCode(ErrorCodes.USERNAME_ALREADY_EXISTS);

            RuleFor(command => command.Email)
                .NotEmpty().WithErrorCode(ErrorCodes.EMAIL_REQUIRED)
                .Matches(GlobalConfiguration.EMAIL_REGEX).WithErrorCode(ErrorCodes.EMAIL_WRONG_FORMAT)
                .Must(NotMatchAnotherEmail).WithErrorCode(ErrorCodes.EMAIL_ALREADY_EXISTS);

            RuleFor(command => command.Password)
                .NotEmpty().WithErrorCode("obligatoire")
                .MinimumLength(GlobalConfiguration.PASSWORD_MIN_LENGHT).WithErrorCode(ErrorCodes.PASSWORD_TOO_SHORT)
                .MaximumLength(GlobalConfiguration.PASSWORD_MAX_LENGHT).WithErrorCode(ErrorCodes.PASSWORD_TOO_LONG)
                .Matches(GlobalConfiguration.PASSWORD_REGEX).WithErrorCode(ErrorCodes.PASSWORD_WRONG_FORMAT);
        }

        private bool NotMatchAnotherUsername(string username) => dataService.NotExists<User>(u => u.Username == username);

        private bool NotMatchAnotherEmail(string email) => dataService.NotExists<User>(u => u.Email == email);
    }

    public class Handler : ICommandHandler<RegisterCommand>
    {
        private readonly IDataService dataService;
        private readonly IPublisher publisher;
        private readonly ILogger<Handler> logger;

        public Handler(IDataService dataService, IPublisher publisher, ILogger<Handler> logger)
        {
            this.dataService = dataService;
            this.publisher = publisher;
            this.logger = logger;
        }
        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            using (var context = dataService.Context())
            {
                await dataService.InsertAsync(new User
                {
                    Email = request.Email,
                    Username = request.UserName,
                    Password = Hash.Create(request.Password),
                    Creation = DateTime.Now,
                    Role = UserRoles.User,
                }, cancellationToken);
                await context.SaveChanges(cancellationToken);
            }
            await publisher.Publish(new UserRegisteredNotification
            {
                UserName = request.UserName,
            }, cancellationToken);
            logger.LogRegistration(request.UserName);
        }
    }
}

internal static partial class LoggerMessageDefinitions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "user {Username} successfully registered")]
    internal static partial void LogRegistration(this ILogger logger, string username);
}