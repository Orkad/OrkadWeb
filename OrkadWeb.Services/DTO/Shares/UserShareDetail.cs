using AutoMapper;
using OrkadWeb.Models;
using OrkadWeb.Services.DTO.Expenses;
using OrkadWeb.Services.DTO.Operations;
using OrkadWeb.Services.DTO.Refunds;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Services.DTO.Shares
{
    public class UserShareDetail
    {
        /// <summary>
        /// Identifiant de l'utilisateur
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dépense de l'utilisateur sur le partage
        /// </summary>
        public List<ExpenseItem> Expenses { get; set; } = new List<ExpenseItem>();

        /// <summary>
        /// Liste des opérations (dépenses, remboursements émits et reçus)
        /// </summary>
        public List<RefundItem> Refunds { get; set; } = new List<RefundItem>();
    }

    public class ShareProfile : Profile
    {
        public ShareProfile()
        {
            CreateMap<UserShare, UserShareDetail>()
                .ForMember(usd => usd.Id, o => o.MapFrom(us => us.User.Id))
                .ForMember(usd => usd.Name, o => o.MapFrom(us => us.User.Username))
                .ForMember(usd => usd.Expenses, o => o.MapFrom(us => us.Expenses))
                .ForMember(usd => usd.Refunds, o => o.MapFrom(us => us.EmittedRefunds.Union(us.ReceivedRefunds)));
        }

    }
}
