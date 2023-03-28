using MediatR;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Exceptions;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Application.Users;
using System.Threading;
using System.Threading.Tasks;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Common;
using OrkadWeb.Application.Common.Exceptions;

namespace OrkadWeb.Application.MonthlyTransactions.Commands
{
    public class DeleteChargeCommand : ICommand
    {
        public int Id { get; set; }

        public class Handler : ICommandHandler<DeleteChargeCommand>
        {
            private readonly IDataService dataService;
            private readonly IAppUser authenticatedUser;

            public Handler(IDataService dataService, IAppUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Unit> Handle(DeleteChargeCommand command, CancellationToken cancellationToken)
            {
                using var context = dataService.Context();
                var transaction = await dataService.GetAsync<MonthlyTransaction>(command.Id, cancellationToken);
                authenticatedUser.MustOwns(transaction);
                if (!transaction.IsCharge())
                {
                    throw new InvalidDataException();
                }
                await dataService.DeleteAsync(transaction, cancellationToken);
                await context.SaveChanges(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
