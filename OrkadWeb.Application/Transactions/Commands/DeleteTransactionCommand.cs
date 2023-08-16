namespace OrkadWeb.Application.Expenses.Commands;

public class DeleteTransactionCommand : ICommand
{
    public int Id { get; init; }

    public class Handler : ICommandHandler<DeleteTransactionCommand>
    {
        private readonly IDataService dataService;

        public Handler(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var transaction = await dataService.GetAsync<Transaction>(request.Id, cancellationToken);
            await dataService.DeleteAsync(transaction, cancellationToken);
            await context.SaveChanges(cancellationToken);
        }
    }
}