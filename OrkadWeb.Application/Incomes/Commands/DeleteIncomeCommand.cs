namespace OrkadWeb.Application.MonthlyTransactions.Commands;

public class DeleteIncomeCommand : ICommand
{
    public int Id { get; init; }

    public class Handler : ICommandHandler<DeleteIncomeCommand>
    {
        private readonly IDataService dataService;

        public Handler(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task Handle(DeleteIncomeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var income = await dataService.GetAsync<Income>(command.Id, cancellationToken);
            await dataService.DeleteAsync(income, cancellationToken);
            await context.SaveChanges(cancellationToken);
        }
    }
}