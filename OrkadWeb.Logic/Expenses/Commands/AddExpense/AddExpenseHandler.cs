using FluentValidation;
using MediatR;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Abstractions;
using OrkadWeb.Logic.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Expenses.Commands.AddExpense
{
    public class AddExpenseHandler : IRequestHandler<AddExpenseCommand, AddExpenseResult>
    {
        private readonly IDataService dataService;
        private readonly IAuthenticatedUser authenticatedUser;

        public AddExpenseHandler(IDataService dataService, IAuthenticatedUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<AddExpenseResult> Handle(AddExpenseCommand command, CancellationToken cancellationToken)
        {
            var expense = new Expense
            {
                Amount = command.Amount,
                Date = DateTime.Now,
                Name = command.Name,
                Owner = dataService.Load<User>(authenticatedUser.Id),
            };
            await dataService.InsertAsync(expense);
            return new AddExpenseResult
            {
                Id = expense.Id,
            };
        }
    }
}
