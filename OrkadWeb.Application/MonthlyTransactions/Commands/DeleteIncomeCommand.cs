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
    public class DeleteIncomeCommand : ICommand
    {
        public int Id { get; set; }

        public class Handler : ICommandHandler<DeleteIncomeCommand>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Unit> Handle(DeleteIncomeCommand command, CancellationToken cancellationToken)
            {
                var transaction = await dataService.GetAsync<MonthlyTransaction>(command.Id);
                authenticatedUser.MustOwns(transaction);
                if (!transaction.IsIncome())
                {
                    throw new InvalidDataException();
                }
                await dataService.DeleteAsync(transaction);
                return Unit.Value;
            }
        }
    }
}
