using OrkadWeb.Models;

namespace OrkadWeb.Services.DTO.Shares
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
        /// Nom du partage
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nombre de participants
        /// </summary>
        public int AttendeeCount { get; set; }

        public static ShareItem BuildFrom(Share share) => share.ToItem();
    }

    public static class ShareItemExtensions
    {
        public static ShareItem ToItem(this Share share)
            => new ShareItem
            {
                Id = share.Id,
                Name = share.Name,
                AttendeeCount = share.UserShares.Count
            };
    }
}