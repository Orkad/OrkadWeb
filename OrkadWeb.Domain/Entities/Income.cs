using OrkadWeb.Domain.Primitives;

namespace OrkadWeb.Domain.Entities
{
    /// <summary>
    /// Monthly income
    /// </summary>
    public class Income : IOwnable
    {
        /// <summary>
        /// Unique identifier of the monthly income
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Amount of the income (always positive)
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Given name of the monthly income
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Owner of the monthly income
        /// </summary>
        public virtual User Owner { get; set; }
    }
}
