namespace OrkadWeb.Application.MonthlyTransactions.Commands;

public class AddIncomeCommand : ICommand<int>
{
    public string Name { get; set; }
    public decimal Amount { get; set; }

    public class Validator : AbstractValidator<AddIncomeCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }

    public class Handler : ICommandHandler<AddIncomeCommand, int>
    {
        private readonly IAppUser authenticatedUser;
        private readonly IDataService dataService;

        public Handler(IDataService dataService, IAppUser authenticatedUser)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
        }

        public async Task<int> Handle(AddIncomeCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var income = new Income
            {
                Name = command.Name,
                Amount = command.Amount,
                Owner = dataService.Load<User>(authenticatedUser.Id)
            };
            await dataService.InsertAsync(income, cancellationToken);
            await context.SaveChanges(cancellationToken);
            return income.Id;
        }
    }
}