namespace OrkadWeb.Application.Charges.Models;

public record ChargeDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }
}