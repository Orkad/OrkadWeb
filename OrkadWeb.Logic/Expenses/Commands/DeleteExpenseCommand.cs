using MediatR;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.CQRS;
using OrkadWeb.Logic.Users;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Expenses.Commands
{
    public class DeleteExpenseCommand : ICommand
    {
        public int Id { get; set; }

        public class Handler : ICommandHandler<DeleteExpenseCommand>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
            {
                var transaction = await dataService.GetAsync<Transaction>(request.Id);
                if (transaction.Owner.Id != authenticatedUser.Id)
                {
                    return Unit.Value;
                }
                await dataService.DeleteAsync(transaction);
                return Unit.Value;
            }
        }
    }
}
