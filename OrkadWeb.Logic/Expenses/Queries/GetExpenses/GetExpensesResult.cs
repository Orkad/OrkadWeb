using System;
using System.Collections.Generic;

namespace OrkadWeb.Logic.Expenses.Queries.GetExpenses
{
    public class GetExpensesResult
    {
        public class ExpenseRow
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string Name { get; set; }
            public decimal Amount { get; set; }
        }

        public List<ExpenseRow> Rows { get; set; }
    }
}
