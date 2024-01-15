using OrkadWeb.Domain.Entities;

namespace OrkadWeb.Domain.Primitives
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
}
