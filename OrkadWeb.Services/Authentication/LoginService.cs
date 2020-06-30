using OrkadWeb.Models;
using OrkadWeb.Services.Data;
using System.Linq;

namespace OrkadWeb.Services.Authentication
{
    public class LoginService : ILoginService
    {
        private readonly IDataService dataService;

        public LoginService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public LoginResult Login(LoginCredentials credentials)
        {
            var hash = HashUtils.HashSHA256(credentials.Password);
            var user = dataService.Query<User>()
                .Where(u => (u.Username == credentials.Username || u.Email == credentials.Username) && u.Password == hash)
                .SingleOrDefault();
            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Error = "La combinaison 'Nom d'utilisateur' & 'Mot de passe' est incorrecte",
                };
            }
            return new LoginResult
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