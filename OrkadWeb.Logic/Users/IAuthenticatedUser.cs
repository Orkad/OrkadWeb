namespace OrkadWeb.Logic.Users
{
    /// <summary>
    /// Représente un utilisateur connecté sur notre application
    /// </summary>
    public interface IAuthenticatedUser
    {
        /// <summary>
        /// L'identifiant unique de l'utilisateur
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Nom d'affichage de l'utilisateur
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Adresse email de contact associé à l'utilisateur
        /// </summary>
        string Email { get; }
    }
}
