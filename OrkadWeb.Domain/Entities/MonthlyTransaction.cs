namespace OrkadWeb.Domain.Entities
{
    /// <summary>
    /// Monthly charge or income
    /// </summary>
    public class MonthlyTransaction : IOwnable
    {
        /// <summary>
        /// Unique identifier of the monthly charge
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Amount (positive for income, negative for charge)
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Given name of the monthly income
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Owner of the monthly transaction
        /// </summary>
        public virtual User Owner { get; set; }

        public virtual bool IsCharge() => Amount < 0;
        public virtual bool IsIncome() => Amount > 0;
    }
}
