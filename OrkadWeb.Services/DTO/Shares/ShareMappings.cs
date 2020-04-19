using OrkadWeb.Models;
using System.Linq;
using OrkadWeb.Services.DTO.Expenses;
using OrkadWeb.Services.DTO.Common;

namespace OrkadWeb.Services.DTO.Shares
{
    public static class ShareMappings
    {
        public static Share ToEntity(this ShareCreation shareCreation) => new Share
        {
            Name = shareCreation.Name
        };

        public static UserShareDetail ToDetail(this UserShare userShare) => new UserShareDetail
        {
            Id = userShare.User.Id,
            Name = userShare.User.Username,
            Expenses = userShare.Expenses.Select(e => e.ToItem()).ToList()
        };

        public static ShareDetail ToDetail(this Share share) => new ShareDetail
        {
            Id = share.Id,
            Name = share.Name,
            Users = share.UserShares.Select(s => s.ToDetail()).ToList()
        };

        public static ShareItem ToItem(this Share share) => new ShareItem
        {
            Id = share.Id,
            Name = share.Name,
            AttendeeCount = share.UserShares.Count
        };
    }
}