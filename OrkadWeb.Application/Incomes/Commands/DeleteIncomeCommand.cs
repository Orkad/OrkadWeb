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

        public async Task Handle(DeleteIncomeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var income = await dataService.GetAsync<Income>(command.Id, cancellationToken);
            authenticatedUser.MustOwns(income);
            await dataService.DeleteAsync(income, cancellationToken);
            await context.SaveChanges(cancellationToken);
        }
    }
}