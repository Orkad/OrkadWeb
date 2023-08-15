namespace OrkadWeb.Application.MonthlyTransactions.Commands;

public class EditIncomeCommand : ICommand
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }

    public class Validator : AbstractValidator<EditIncomeCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }

    public class Handler : ICommandHandler<EditIncomeCommand>
    {
        private readonly IDataService dataService;

        public Handler(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task Handle(EditIncomeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var income = await dataService.GetAsync<Income>(command.Id, cancellationToken);
            income.Name = command.Name;
            income.Amount = command.Amount;
            await context.SaveChanges(cancellationToken);
        }
    }
}