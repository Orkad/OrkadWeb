using OrkadWeb.Models;
using OrkadWeb.Services.DTO.Common;
using OrkadWeb.Services.DTO.Expenses;
using OrkadWeb.Services.DTO.Refunds;
using OrkadWeb.Services.DTO.Shares;
using OrkadWeb.Services.DTO.Users;
using OrkadWeb.Services.Exceptions;
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
        /// Récupére les autres utilisateur du partage que celui passé en paramètre
        /// </summary>
        /// <param name="userId">identifiant unique de l'utilisateur</param>
        /// <param name="shareId">identifiant unique du partage</param>
        public List<TextValue> GetOtherUsers(int userId, int shareId)
        {
            var userShare = GetUserShare(userId, shareId);
            var query = userShare.Share.UserShares
                .Select(us => us.User)
                .Where(u => u.Id != userId)
                .Select(u => u.ToTextValue());
            var result = query.ToList();
            return result;
        }

        /// <summary>
        /// Ajoute un remboursement pour l'utilisateur sur le partage renseigné
        /// </summary>
        /// <param name="userId">identifiant de l'utilisateur effectuant le remboursement</param>
        /// <param name="shareId">identifiant du partage concerné</param>
        /// <param name="refundCreation">données liés au remboursement</param>
        /// <returns></returns>
        public RefundItem AddRefund(int userId, int shareId, RefundCreation refundCreation)
        {
            var emmiterUserShare = GetUserShare(userId, shareId);
            var receiverUserShare = GetUserShare(refundCreation.Receiver, shareId);
            var refund = new Refund
            {
                Amount = refundCreation.Amount,
                Emitter = emmiterUserShare,
                Receiver = receiverUserShare,
            };
            dataService.Insert(refund);
            return refund.ToItem();
        }

        /// <summary>
        /// Créé un nouveau partage utilisateur
        /// </summary>
        /// <param name="userId">utilisateur réalisant la création</param>
        /// <param name="shareCreation">donnée concernant le nouveau partage a créer</param>
        public ShareItem CreateShareForUser(int userId, ShareCreation shareCreation)
        {
            var user = dataService.Load<User>(userId);
            var share = shareCreation.ToEntity();
            share.Owner = user;
            // On ajoute automatiquement l'utilisateur a son partage
            share.UserShares = new HashSet<UserShare>()
            {
                new UserShare
                {
                    Share = share,
                    User = user,
                }
            };
            dataService.Insert(share);
            return share.ToItem();
        }

        /// <summary>
        /// Supprime un partage utilisateur existant
        /// </summary>
        /// <param name="userId">utilisateur réalisant la suppression</param>
        /// <param name="shareId">identifiant unique du partage a supprimer</param>
        public void DeleteShare(int userId, int shareId)
        {
            var share = dataService.Get<Share>(shareId);
            if (share == null)
            {
                throw new BusinessException($"Impossible de supprimer le partage n°{shareId} car celui ci n'existe pas");
            }
            if (share.Owner?.Id != userId)
            {
                throw new BusinessException($"Impossible de supprimer le partage n°{shareId} car vous n'en êtes pas le propriétaire");
            }
            dataService.Delete(share);
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
            detail.OwnerId = share.Owner.Id;
            return detail;
        }

        /// <summary>
        /// Supprime une dépense d'un utilisateur sur un partage
        /// </summary>
        /// <param name="userId">identifiant de l'utilisateur concerné</param>
        /// <param name="shareId">identifiant du partage</param>
        /// <param name="expenseId">identifiant unique de la dépense a supprimer</param>
        /// <returns></returns>
        public void DeleteExpense(int userId, int shareId, int expenseId)
        {
            var userShare = GetUserShare(userId, shareId);
            var expense = userShare.Expenses.SingleOrDefault(e => e.Id == expenseId);
            if (expense == null)
            {
                throw new BusinessException($"La dépense n°{expenseId} est introuvable sur le partage {userShare.Share.Name}");
            }
            userShare.Expenses.Remove(expense);
            dataService.Delete(expense);
        }

        /// <summary>
        /// Ajoute une dépense d'un utilisateur sur un partage
        /// </summary>
        /// <param name="userId">identifiant de l'utilisateur concerné</param>
        /// <param name="shareId">identifiant du partage</param>
        /// <param name="creation">donnée de la dépense</param>
        public ExpenseItem AddExpense(int userId, int shareId, ExpenseCreation expenseCreation)
        {
            var userShare = GetUserShare(userId, shareId);
            var expense = expenseCreation.ToEntity(userShare, DateTime.Now);
            dataService.Insert(expense);
            return expense.ToItem();
        }

        /// <summary>
        /// Récupère le montant total donné pour le partage en comptant les dépenses et les remboursement
        /// </summary>
        /// <param name="userId">identifiant unique de l'utilisateur</param>
        /// <param name="shareId">identifiant unique du partage</param>
        public decimal GetUserBalance(int userId, int shareId)
        {
            var userShare = GetUserShare(userId, shareId);
            var expenses = userShare.Expenses.Sum(e => e.Amount) + userShare.EmittedRefunds.Sum(e => e.Amount);
            var gains = userShare.ReceivedRefunds.Sum(e => e.Amount);
            var balance = expenses - gains;
            return balance;
        }

        /// <summary>
        /// Récupère un partage utilisateur existant
        /// </summary>
        /// <param name="userId">identifiant unique d'un utilisateur</param>
        /// <param name="shareId">identifiant unique d'un partage</param>
        private UserShare GetUserShare(int userId, int shareId)
        {
            var userShare = dataService.Query<UserShare>()
                .Where(us => us.User.Id == userId && us.Share.Id == shareId)
                .SingleOrDefault();
            if (userShare == null)
            {
                throw new ArgumentException($"Aucun partage utilisateur n'a pu être trouvé pour userId={userId} & shareId={shareId}");
            }
            return userShare;
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
