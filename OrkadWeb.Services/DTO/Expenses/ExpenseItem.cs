using OrkadWeb.Models;

namespace OrkadWeb.Services.DTO.Expenses
{
    public class ExpenseItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }

    public static class ExpenseItemMappings
    {
        public static ExpenseItem ToItem(this Expense expense)
            => new ExpenseItem
            {
                Id = expense.Id,
                Name = expense.Name,
                Amount = expense.Amount
            };
    }
}