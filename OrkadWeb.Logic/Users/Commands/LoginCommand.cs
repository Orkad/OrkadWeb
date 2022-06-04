using MediatR;
using NHibernate.Linq;
using OrkadWeb.Common;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
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

            public Handler(IDataService dataService)
            {
                this.dataService = dataService;
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
                return new Result
                {
                    Success = true,
                    Id = user.Id.ToString(),
                    Name = user.Username,
                    Email = user.Email,
                    Role = "User",
                };
            }
        }

    }
}
