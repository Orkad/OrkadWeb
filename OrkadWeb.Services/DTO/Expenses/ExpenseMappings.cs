using OrkadWeb.Models;

namespace OrkadWeb.Services.DTO.Expenses
{
    public static class ExpenseMappings
    {
        public static ExpenseItem ToItem(this Expense expense)
            => new ExpenseItem
            {
                Id = expense.Id,
                Name = expense.Name,
                Amount = expense.Amount,
                Date = expense.Date,
            };

        public static Expense ToEntity(this ExpenseCreation creation, UserShare userShare)
            => new Expense
            {
                Name = creation.Name,
                Amount = creation.Amount,
                Date  = creation.Date,
                UserShare = userShare,
            };
    }
}