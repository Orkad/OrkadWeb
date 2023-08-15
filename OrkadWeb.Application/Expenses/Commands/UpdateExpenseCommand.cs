using System;

namespace OrkadWeb.Application.Expenses.Commands;

public class UpdateExpenseCommand : ICommand
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }

    public class Handler : ICommandHandler<UpdateExpenseCommand>
    {
        private readonly IDataService dataService;

        public Handler(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var transaction = await dataService.GetAsync<Transaction>(request.Id, cancellationToken);
            transaction.Date = request.Date;
            transaction.Name = request.Name;
            transaction.Amount = request.Amount;
            await context.SaveChanges(cancellationToken);
        }
    }
}