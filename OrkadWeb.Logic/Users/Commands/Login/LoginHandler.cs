using MediatR;
using NHibernate.Linq;
using OrkadWeb.Common;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Users.Commands
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IDataService dataService;

        public LoginHandler(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await dataService.Query<User>()
                .Where(u => u.Username == request.Username || u.Email == request.Username)
                .SingleOrDefaultAsync(cancellationToken);
            if (user == null || !Hash.Validate(request.Password, user.Password))
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "La combinaison 'Nom d'utilisateur' & 'Mot de passe' est incorrecte",
                };
            }
            return new LoginResponse
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
