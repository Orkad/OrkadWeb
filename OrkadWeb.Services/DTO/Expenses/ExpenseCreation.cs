using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Services.DTO.Expenses
{
    public class ExpenseCreation
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
