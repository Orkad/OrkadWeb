namespace OrkadWeb.Application.Incomes.Commands;

public class CreateIncome : ICommand<int>
{
    public string Name { get; init; }
    public decimal Amount { get; init; }

    public class Validator : AbstractValidator<CreateIncome>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }

    public class Handler(IDataService dataService, IAppUser authenticatedUser) : ICommandHandler<CreateIncome, int>
    {
        public async Task<int> Handle(CreateIncome command, CancellationToken cancellationToken)
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