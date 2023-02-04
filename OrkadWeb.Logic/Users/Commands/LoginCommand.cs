using NHibernate.Linq;
using OrkadWeb.Common;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.CQRS;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using static OrkadWeb.Logic.Users.Commands.LoginCommand;

namespace OrkadWeb.Logic.Users.Commands
{
    public class LoginCommand : ICommand<Result>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class Result
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
            public bool Success { get; set; }
            public string Error { get; set; }
            public string Token { get; set; }
        }

        public class Handler : ICommandHandler<LoginCommand, Result>
        {
            private readonly IDataService dataService;
            private readonly IIdentityTokenGenerator identityTokenGenerator;

            public Handler(IDataService dataService, IIdentityTokenGenerator identityTokenGenerator)
            {
                this.dataService = dataService;
                this.identityTokenGenerator = identityTokenGenerator;
            }

            public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await dataService.Query<User>()
                    .Where(u => u.Username == request.Username || u.Email == request.Username)
                    .SingleOrDefaultAsync(cancellationToken);
                if (user == null || !Hash.Validate(request.Password, user.Password))
                {
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
                    Role = "User",
                    Token = identityTokenGenerator.GenerateToken(GetClaims(user.Id.ToString(), user.Username, user.Email)),
                };
                return result;
            }

            private Claim[] GetClaims(string id, string name, string email)
            {
                return new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.Sub, id),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Name, name),
                };
            }
        }

    }
}
