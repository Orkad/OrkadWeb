using OrkadWeb.Models;
using OrkadWeb.Services.DTO.Expenses;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Services.DTO.Shares
{
    public class UserShareDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ExpenseItem> Expenses { get; set; } = new List<ExpenseItem>();
    }

    public static class UserShareDetailExtensions
    {
        public static UserShareDetail ToDetail(this UserShare userShare)
            => new UserShareDetail
            {
                Id = userShare.Id,
                Name = userShare.User.Username,
                Expenses = userShare.Expenses.Select(e => e.ToItem()).ToList()
            };
    }
}
