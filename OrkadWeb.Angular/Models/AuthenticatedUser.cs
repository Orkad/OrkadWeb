using OrkadWeb.Logic.Users;

namespace OrkadWeb.Angular.Models
{
    /// <summary>
    /// Représente un utilisateur connecté sur l'api
    /// </summary>
    class AuthenticatedUser : IAuthenticatedUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
