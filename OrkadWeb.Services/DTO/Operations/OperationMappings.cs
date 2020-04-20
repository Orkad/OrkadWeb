using OrkadWeb.Models;
using System;

namespace OrkadWeb.Services.DTO.Operations
{
    public static class OperationMappings
    {
        public static OperationItem ToOperationItem(this Expense expense) => new OperationItem
        {
            Name = expense.Name,
            Amount = expense.Amount,
            Date = expense.Date,
        };

        public static OperationItem ToOperationItem(this Refund refund, bool emitted) => new OperationItem
        {
            Name = emitted ? "Remboursement pour " + refund.Receiver.User.Username : "Remboursement de " + refund.Emitter.User.Username,
            Amount = emitted ? -refund.Amount : refund.Amount,
            Date = DateTime.Now, // TODO store date in refund
        };
    }
}
