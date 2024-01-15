namespace OrkadWeb.Application.Incomes.Commands;

public class DeleteIncome : ICommand
{
    public int Id { get; init; }

    public class Handler(IDataService dataService) : ICommandHandler<DeleteIncome>
    {
        public async Task Handle(DeleteIncome command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var income = await dataService.GetAsync<Income>(command.Id, cancellationToken);
            await dataService.DeleteAsync(income, cancellationToken);
            await context.SaveChanges(cancellationToken);
        }
    }
}