namespace OrkadWeb.Application.Charges.Commands;

public class EditChargeCommand : ICommand
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Amount { get; init; }

    public class Validator : AbstractValidator<EditChargeCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Le montant de la charge doit être supérieur à 0");;
        }
    }

    public class Handler : ICommandHandler<EditChargeCommand>
    {
        private readonly IDataService dataService;

        public Handler(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task Handle(EditChargeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var monthlyTransaction = await dataService.GetAsync<Charge>(command.Id, cancellationToken);
            monthlyTransaction.Name = command.Name;
            monthlyTransaction.Amount = command.Amount;
            await context.SaveChanges(cancellationToken);
        }
    }
}