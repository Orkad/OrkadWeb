namespace OrkadWeb.Domain.Entities
{
    /// <summary>
    /// Owned by a user.
    /// </summary>
    public interface IOwnable
    {
        /// <summary>
        /// Owner of this.
        /// </summary>
        User Owner { get; }
    }

    /// <summary>
    /// Defines extension methods for <see cref="IOwnable"/>
    /// </summary>
    public static class IOwnableExtensions
    {
        /// <summary>
        /// Check if it is owned by the given user provided by his id.
        /// </summary>
        public static bool IsOwnedBy(this IOwnable ownable, int userId) => ownable.Owner.Id == userId;
    }
}
