using OrkadWeb.Models;
using OrkadWeb.Services.DTO.Shares;
using System;
using System.Collections.Generic;
using System.Linq;

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
                .Where(us => us.User.Id == userId).Select(us => us.Share);
            var result = query.Select(ShareItem.BuildFrom).ToList();
            return result;
        }

        /// <summary>
        /// Récupère le détail du partage
        /// </summary>
        /// <param name="shareId">identifiant unique du partage</param>
        /// <returns></returns>
        public ShareDetail GetShareDetail(int shareId)
        {
            var share = dataService.Get<Share>(shareId);
            var detail = share.ToDetail();
            return detail;
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
            return dataService.Query<UserShare>()
                .Where(us => us.Share.Id == shareId)
                .Select(us => us.Expenses.Sum(e => e.Amount)).Sum();
        }
    }
}
