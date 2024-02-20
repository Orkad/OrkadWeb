namespace OrkadWeb.Application.Incomes.Commands;

public class UpdateIncome : ICommand
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }

    public class Validator : AbstractValidator<UpdateIncome>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }

    public class Handler(IDataService dataService) : ICommandHandler<UpdateIncome>
    {
        public async Task Handle(UpdateIncome command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var income = await dataService.GetAsync<Income>(command.Id, cancellationToken);
            income.Name = command.Name;
            income.Amount = command.Amount;
            await context.SaveChanges(cancellationToken);
        }
    }
}