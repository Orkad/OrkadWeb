namespace OrkadWeb.Logic.Shares.Models
{
    /// <summary>
    /// Représente un partage dans une collection
    /// </summary>
    public class ShareItem
    {
        /// <summary>
        /// Identifiant unique du partage
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifiant de l'utilisateur propriétaire du partage
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Nom du partage
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nombre de participants
        /// </summary>
        public int UserCount { get; set; }
    }
}