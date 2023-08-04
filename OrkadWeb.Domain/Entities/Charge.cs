namespace OrkadWeb.Domain.Entities
{
    /// <summary>
    /// Monthly charge
    /// </summary>
    public class Charge : IOwnable
    {
        /// <summary>
        /// Unique identifier of the charge
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Amount of the charge (always positive)
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Name of the charge
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Owner of the charge
        /// </summary>
        public virtual User Owner { get; set; }
    }
}
