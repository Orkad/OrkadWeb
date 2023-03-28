using FluentValidation;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Application.Users;
using System.Threading;
using System.Threading.Tasks;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Common;

namespace OrkadWeb.Application.MonthlyTransactions.Commands
{
    public class AddIncomeCommand : ICommand<int>
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public class Validator : AbstractValidator<AddIncomeCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Amount).GreaterThan(0);
            }
        }
        public class Handler : ICommandHandler<AddIncomeCommand, int>
        {
            private readonly IDataService dataService;
            private readonly IAppUser authenticatedUser;

            public Handler(IDataService dataService, IAppUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }
            public async Task<int> Handle(AddIncomeCommand command, CancellationToken cancellationToken)
            {
                using var context = dataService.Context();
                var monthlyTransaction = new MonthlyTransaction
                {
                    Name = command.Name,
                    Amount = command.Amount,
                    Owner = dataService.Load<User>(authenticatedUser.Id),
                };
                await dataService.InsertAsync(monthlyTransaction, cancellationToken);
                await context.SaveChanges(cancellationToken);
                return monthlyTransaction.Id;
            }
        }
    }
}
