using System;

namespace OrkadWeb.Application.Transactions.Commands;

public class UpdateTransactionExpenseCommand : ICommand
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }

    public class Handler : ICommandHandler<UpdateTransactionExpenseCommand>
    {
        private readonly IDataService dataService;

        public Handler(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task Handle(UpdateTransactionExpenseCommand request, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var transaction = await dataService.GetAsync<Transaction>(request.Id, cancellationToken);
            transaction.Date = request.Date;
            transaction.Name = request.Name;
            transaction.Amount = -request.Amount;
            await context.SaveChanges(cancellationToken);
        }
    }
}