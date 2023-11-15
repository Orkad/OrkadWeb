using System;

namespace OrkadWeb.Application.Transactions.Commands;

public class AddTransactionGainCommand : ICommand<int>
{
    /// <summary>
    /// Montant de la dépense
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Nom d'affichage de la dépense
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Date de la dépense (optionnel)
    /// </summary>
    public DateTime? Date { get; init; }

    public class Validator : AbstractValidator<AddTransactionGainCommand>
    {
        public Validator(ITimeProvider timeProvider)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(command => command.Amount).GreaterThan(0)
                .WithMessage("Le montant doit être supérieur à 0€");
            RuleFor(command => command.Name).NotEmpty()
                .WithMessage("Le nom du gain doit être défini");
            RuleFor(command => command.Date).LessThan(timeProvider.Now).When(command => command.Date.HasValue)
                .WithMessage("La date doit être inférieure à la date du jour");
        }
    }

    public class Handler : ICommandHandler<AddTransactionGainCommand, int>
    {
        private readonly IDataService dataService;
        private readonly ITimeProvider timeProvider;

        public Handler(IDataService dataService, ITimeProvider timeProvider)
        {
            this.dataService = dataService;
            this.timeProvider = timeProvider;
        }

        public async Task<int> Handle(AddTransactionGainCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var transaction = new Transaction
            {
                Name = command.Name,
                Amount = command.Amount,
                Date = command.Date ?? timeProvider.Now,
            };
            await dataService.InsertAsync(transaction, cancellationToken);
            await context.SaveChanges(cancellationToken);
            return transaction.Id;
        }
    }
}