using OrkadWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Services.DTO.Shares
{
    public class ShareDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserShareDetail> Users { get; set; } = new List<UserShareDetail>();


        public decimal TotalExpenses => Users.Sum(u => u.Expenses.Sum(e => e.Amount));
    }

    public static class ShareDetailExtensions
    {
        public static ShareDetail ToDetail(this Share share)
            => new ShareDetail
            {
                Id = share.Id,
                Name = share.Name,
                Users = share.UserShares.Select(s => s.ToDetail()).ToList()
            };
    }
}
