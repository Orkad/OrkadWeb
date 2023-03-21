using MediatR;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Application.Users;
using System.Threading;
using System.Threading.Tasks;
using OrkadWeb.Domain.Common;
using OrkadWeb.Application.Common.Interfaces;

namespace OrkadWeb.Application.Expenses.Commands
{
    public class DeleteExpenseCommand : ICommand
    {
        public int Id { get; set; }

        public class Handler : ICommandHandler<DeleteExpenseCommand>
        {
            private readonly IRepository dataService;
            private readonly IAppUser authenticatedUser;

            public Handler(IRepository dataService, IAppUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
            {
                var transaction = await dataService.GetAsync<Transaction>(request.Id);
                authenticatedUser.MustOwns(transaction);
                await dataService.DeleteAsync(transaction);
                return Unit.Value;
            }
        }
    }
}
