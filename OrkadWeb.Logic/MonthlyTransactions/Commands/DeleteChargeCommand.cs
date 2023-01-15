using FluentValidation;
using MediatR;
using OrkadWeb.Data;
using OrkadWeb.Data.Exceptions;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.CQRS;
using OrkadWeb.Logic.Users;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.MonthlyTransactions.Commands
{
    public class DeleteChargeCommand : ICommand
    {
        public int ChargeId { get; set; }

        public class Handler : ICommandHandler<DeleteChargeCommand>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Unit> Handle(DeleteChargeCommand command, CancellationToken cancellationToken)
            {
                var transaction = await dataService.GetAsync<MonthlyTransaction>(command.ChargeId);
                authenticatedUser.MustOwns(transaction);
                if (!transaction.IsCharge())
                {
                    throw new DataNotFoundException<MonthlyTransaction>(command.ChargeId);
                }
                await dataService.DeleteAsync(transaction);
                return Unit.Value;
            }
        }
    }

    public class AddChargeCommand : ICommand<int>
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public class Validator : AbstractValidator<AddChargeCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Amount).GreaterThan(0);
            }
        }

        public class Handler : ICommandHandler<AddChargeCommand, int>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }
            public async Task<int> Handle(AddChargeCommand command, CancellationToken cancellationToken)
            {
                var monthlyTransaction = new MonthlyTransaction
                {
                    Name = command.Name,
                    Amount = -command.Amount, //negative
                    Owner = dataService.Load<User>(authenticatedUser.Id),
                };
                await dataService.InsertAsync(monthlyTransaction);
                return monthlyTransaction.Id;
            }
        }
    }
}
