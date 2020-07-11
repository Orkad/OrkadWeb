using AutoMapper;
using OrkadWeb.Models;
using OrkadWeb.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Services.DTO.Shares
{
    public class ShareDetail : Ownable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserShareDetail> Users { get; set; } = new List<UserShareDetail>();
        public ShareRule Rule { get; set; }


        public decimal TotalExpenses => Users.Sum(u => u.Expenses.Sum(e => e.Amount));
    }
}
