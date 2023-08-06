namespace OrkadWeb.Application.MonthlyTransactions.Commands;

public class AddChargeCommand : ICommand<int>
{
    public string Name { get; set; }
    public decimal Amount { get; set; }

    public class Validator : AbstractValidator<AddChargeCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Le nom de la charge ne doit pas être vide");
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Le montant de la charge doit être supérieur à 0");
        }
    }

    public class Handler : ICommandHandler<AddChargeCommand, int>
    {
        private readonly IAppUser authenticatedUser;
        private readonly IDataService dataService;

        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<int> Handle(AddChargeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var monthlyTransaction = new Charge
            {
                Name = command.Name,
                Amount = command.Amount,
                Owner = dataService.Load<User>(authenticatedUser.Id)
            };
            await dataService.InsertAsync(monthlyTransaction, cancellationToken);
            await context.SaveChanges(cancellationToken);
            return monthlyTransaction.Id;
        }
    }
}