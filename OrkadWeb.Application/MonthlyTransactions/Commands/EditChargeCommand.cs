using FluentValidation;
using MediatR;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Application.Users;
using System.Threading;
using System.Threading.Tasks;
using OrkadWeb.Domain.Common;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Application.Common.Exceptions;

namespace OrkadWeb.Application.MonthlyTransactions.Commands
{
    public class EditChargeCommand : ICommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public class Validator : AbstractValidator<EditChargeCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
                RuleFor(x => x.Amount).GreaterThan(0);
            }
        }

        public class Handler : ICommandHandler<EditChargeCommand>
        {
            private IDataService dataService;
            private IAppUser authenticatedUser;

            public Handler(IDataService dataService, IAppUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Unit> Handle(EditChargeCommand command, CancellationToken cancellationToken)
            {
                var monthlyTransaction = await dataService.GetAsync<MonthlyTransaction>(command.Id, cancellationToken);
                authenticatedUser.MustOwns(monthlyTransaction);
                if (!monthlyTransaction.IsCharge())
                {
                    throw new InvalidDataException();
                }
                monthlyTransaction.Name = command.Name;
                monthlyTransaction.Amount = -command.Amount;
                await dataService.UpdateAsync(monthlyTransaction, cancellationToken);
                return Unit.Value;
            }
        }
    }
}
