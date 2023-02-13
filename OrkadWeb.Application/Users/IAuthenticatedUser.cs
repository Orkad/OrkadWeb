using OrkadWeb.Domain.Entities;
using OrkadWeb.Application.Users.Exceptions;

namespace OrkadWeb.Application.Users
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

    /// <summary>
    /// Defines extension method for <see cref="IAuthenticatedUser"/>
    /// </summary>
    public static class IAuthenticatedUserExtensions
    {
        /// <summary>
        /// Check if the authenticated user owns the given entity.
        /// </summary>
        public static bool Owns(this IAuthenticatedUser user, IOwnable ownable) => ownable.IsOwnedBy(user.Id);

        /// <summary>
        /// Check if the authenticated user owns this given entity.
        /// Otherwise, throws <see cref="NotOwnedException"/>.
        /// </summary>
        /// <exception cref="NotOwnedException"></exception>
        public static void MustOwns(this IAuthenticatedUser user, IOwnable ownable)
        {
            if (!user.Owns(ownable))
            {
                throw new NotOwnedException();
            }
        }
    }
}
