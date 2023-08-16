using System;

namespace OrkadWeb.Application.Transactions.Models;

public class TransactionVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}