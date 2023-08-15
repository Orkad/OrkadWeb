using Microsoft.Extensions.Logging;
using OrkadWeb.Application.Security;
using static OrkadWeb.Application.Users.Commands.LoginCommand;

namespace OrkadWeb.Application.Users.Commands;

public class LoginCommand : ICommand<Result>
{
    public string Username { get; init; }
    public string Password { internal get; init; } // internal for not exposing password

    public class Result
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Success { get; init; }
        public string Error { get; set; }
        public string Token { get; set; }
    }

    public class Handler : ICommandHandler<LoginCommand, Result>
    {
        private readonly IDataService dataService;
        private readonly IIdentityTokenGenerator identityTokenGenerator;
        private readonly ILogger<Handler> logger;

        public Handler(IDataService dataService, IIdentityTokenGenerator identityTokenGenerator, ILogger<Handler> logger)
        {
            this.dataService = dataService;
            this.identityTokenGenerator = identityTokenGenerator;
            this.logger = logger;
        }

        public async Task<Result> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await dataService.FindAsync<User>(u => u.Username == command.Username || u.Email == command.Username, cancellationToken);
            if (user == null || !Hash.Validate(command.Password, user.Password))
            {
                logger.LogAuthenticationFailed(command.Username);
                return new Result
                {
                    Success = false,
                    Error = "Nom d'utilisateur ou mot de passe incorrect",
                };
            }
            var result = new Result
            {
                Success = true,
                Id = user.Id.ToString(),
                Name = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = identityTokenGenerator.GenerateToken(user),
            };
            logger.LogAuthenticationSuccess(user.Username);
            return result;
        }
    }
}

internal static partial class LoggerMessageDefinitions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "user {Username} successfully authenticated")]
    internal static partial void LogAuthenticationSuccess(this ILogger logger, string username);

    [LoggerMessage(Level = LogLevel.Information, Message = "user {Username} failed to authenticate")]
    internal static partial void LogAuthenticationFailed(this ILogger logger, string username);
}