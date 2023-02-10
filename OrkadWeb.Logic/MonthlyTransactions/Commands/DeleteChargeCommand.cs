using MediatR;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Exceptions;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Logic.CQRS;
using OrkadWeb.Logic.Exceptions;
using OrkadWeb.Logic.Users;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.MonthlyTransactions.Commands
{
    public class DeleteChargeCommand : ICommand
    {
        public int Id { get; set; }

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
                var transaction = await dataService.GetAsync<MonthlyTransaction>(command.Id);
                authenticatedUser.MustOwns(transaction);
                if (!transaction.IsCharge())
                {
                    throw new InvalidDataException();
                }
                await dataService.DeleteAsync(transaction);
                return Unit.Value;
            }
        }
    }
}
