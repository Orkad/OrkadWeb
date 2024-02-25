using System;
using Microsoft.Extensions.Logging;
using OrkadWeb.Application.Security;

namespace OrkadWeb.Application.Users.Commands;

public class LoginResult
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool Success { get; init; }
    public string Error { get; set; }
    /// <summary>
    /// JWT
    /// </summary>
    public string Token { get; set; }
}

public class LoginCommand : ICommand<LoginResult>
{
    public string Username { get; init; }
    public string Password { internal get; init; } // internal for not exposing password


    public class Handler : ICommandHandler<LoginCommand, LoginResult>
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

        public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await dataService.FindAsync<User>(u => u.Username == command.Username || u.Email == command.Username, cancellationToken);
            if (user == null || !Hash.Validate(command.Password, user.Password))
            {
                logger.LogAuthenticationFailed(command.Username);
                return new LoginResult
                {
                    Success = false,
                    Error = "Nom d'utilisateur ou mot de passe incorrect",
                };
            }
            var result = new LoginResult
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
