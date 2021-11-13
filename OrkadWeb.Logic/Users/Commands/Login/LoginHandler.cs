using MediatR;
using NHibernate.Linq;
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
            var hash = HashSHA256(request.Password);
            var user = await dataService.Query<User>()
                .Where(u => (u.Username == request.Username || u.Email == request.Username) && u.Password == hash)
                .SingleOrDefaultAsync(cancellationToken);
            if (user == null)
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

        /// <summary>
        /// Permet de crypter en SHA256
        /// </summary>
        /// <param name="value">la valeur a crypter</param>
        /// <returns>la valeur crypter</returns>
        private static string HashSHA256(string value)
        {
            StringBuilder Sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                foreach (var b in hash.ComputeHash(Encoding.UTF8.GetBytes(value)))
                {
                    Sb.Append(b.ToString("x2"));
                }
            }
            return Sb.ToString();
        }
    }
}
