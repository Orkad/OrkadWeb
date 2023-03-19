using OrkadWeb.Application.Users;

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

        public string Role { get; set; }
    }
}
