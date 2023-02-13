using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Application.MonthlyTransactions.Models
{
    public class MonthlyChargeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
