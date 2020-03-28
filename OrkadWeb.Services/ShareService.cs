using OrkadWeb.Models;
using OrkadWeb.Services.DTO.Shares;
using OrkadWeb.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrkadWeb.Services
{
    public class ShareService
    {
        private readonly IDataService dataService;

        public ShareService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// Récupère les partages pour un seul utilisateur
        /// </summary>
        /// <param name="userId">identifiant unique de l'utilisateur</param>
        public List<ShareItem> GetSharesForUser(int userId)
        {
            var query = dataService.Query<UserShare>()
                .Where(us => us.User.Id == userId)
                .Select(us => new ShareItem
                {
                    Id = us.Share.Id,
                    Name = us.Share.Name,
                    AttendeeCount = us.Share.UserShares.Count()
                });
            var result = query.ToList();
            return result;
        }

        /// <summary>
        /// Récupère le montant total donné pour le partage en comptant les dépenses et les remboursement
        /// </summary>
        /// <param name="userId">identifiant unique de l'utilisateur</param>
        /// <param name="shareId">identifiant unique du partage</param>
        public decimal GetUserBalance(int userId, int shareId)
        {
            var userShare = dataService.Query<UserShare>()
                .Where(us => us.User.Id == userId && us.Share.Id == shareId)
                .SingleOrDefault();
            if (userShare == null)
            {
                throw new ArgumentException($"Aucun partage utilisateur n'a pu être trouvé pour userId={userId} & shareId={shareId}");
            }
            var expenses = userShare.Expenses.Sum(e => e.Amount) + userShare.EmittedRefunds.Sum(e => e.Amount);
            var gains = userShare.ReceivedRefunds.Sum(e => e.Amount);
            var balance = expenses - gains;
            return balance;
        }

        /// <summary>
        /// Récupère le montant total dépensé par l'intégralité des utilisateurs
        /// </summary>
        /// <param name="shareId">identifiant unique du partage</param>
        public decimal GetShareExpenses(int shareId)
        {
            var share = dataService.Query<Share>().Where(s => s.Id == shareId);
            if (share == null)
            {

            }
            return 0m;
        }
    }
}
