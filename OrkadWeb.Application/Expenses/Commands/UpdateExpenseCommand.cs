using MediatR;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Application.Users;
using System;
using System.Threading;
using System.Threading.Tasks;
using OrkadWeb.Domain.Common;
using OrkadWeb.Application.Common.Interfaces;

namespace OrkadWeb.Application.Expenses.Commands
{
    public class UpdateExpenseCommand : ICommand
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public class Handler : ICommandHandler<UpdateExpenseCommand>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Unit> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
            {
                var transaction = await dataService.GetAsync<Transaction>(request.Id);
                authenticatedUser.MustOwns(transaction);
                transaction.Date = request.Date;
                transaction.Name = request.Name;
                transaction.Amount = request.Amount;
                await dataService.UpdateAsync(transaction);
                return Unit.Value;
            }
        }
    }
}
