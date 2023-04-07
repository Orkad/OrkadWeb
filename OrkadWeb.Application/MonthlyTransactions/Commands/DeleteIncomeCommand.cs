using OrkadWeb.Application.Common.Exceptions;

namespace OrkadWeb.Application.MonthlyTransactions.Commands;

public class DeleteIncomeCommand : ICommand
{
    public int Id { get; set; }

    public class Handler : ICommandHandler<DeleteIncomeCommand>
    {
        private readonly IAppUser authenticatedUser;
        private readonly IDataService dataService;

        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<Unit> Handle(DeleteIncomeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var transaction = await dataService.GetAsync<MonthlyTransaction>(command.Id, cancellationToken);
            authenticatedUser.MustOwns(transaction);
            if (!transaction.IsIncome()) throw new InvalidDataException();
            await dataService.DeleteAsync(transaction, cancellationToken);
            await context.SaveChanges(cancellationToken);
            return Unit.Value;
        }
    }
}