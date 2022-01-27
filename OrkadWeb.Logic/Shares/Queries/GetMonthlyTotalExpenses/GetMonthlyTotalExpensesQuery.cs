using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Logic.Shares.Queries.GetMonthlyTotalExpenses
{
    public class GetMonthlyTotalExpensesQuery : IQuery<decimal>
    {
        public DateTime Month { get; set; }
    }
}
