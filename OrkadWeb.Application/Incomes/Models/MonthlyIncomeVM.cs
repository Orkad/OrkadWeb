namespace OrkadWeb.Application.MonthlyTransactions.Models;

public record MonthlyIncomeVM
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }
}