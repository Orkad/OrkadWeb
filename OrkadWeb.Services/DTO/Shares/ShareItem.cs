using System;
using System.Text;

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
    }
}
