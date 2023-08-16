using System;

namespace OrkadWeb.Application.Transactions.Commands;

/// <summary>
/// Permet d'ajouter une dépense
/// </summary>
public class AddTransactionExpenseCommand : ICommand<AddTransactionExpenseCommand.Result>
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

    /// <summary>
    /// Résultat de l'ajout d'une dépense
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Identifiant unique de la dépense créée
        /// </summary>
        public int Id { get; set; }
    }

    public class Validator : AbstractValidator<AddTransactionExpenseCommand>
    {
        public Validator(ITimeProvider timeProvider)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(command => command.Amount).GreaterThan(0)
                .WithMessage("Le montant doit être suppérieur à 0€");
            RuleFor(command => command.Name).NotEmpty()
                .WithMessage("Le nom de la dépense doit être défini");
            RuleFor(command => command.Date).LessThan(timeProvider.Now).When(command => command.Date.HasValue)
                .WithMessage("La date doit être inférieure à la date du jour");
        }
    }

    public class Handler : ICommandHandler<AddTransactionExpenseCommand, Result>
    {
        private readonly IDataService dataService;

        public Handler(IDataService repository)
        {
            this.dataService = repository;
        }

        public async Task<Result> Handle(AddTransactionExpenseCommand command, CancellationToken cancellationToken)
        {
            using var context = dataService.Context();
            var transaction = new Transaction
            {
                Amount = -command.Amount,
                Date = command.Date ?? DateTime.Now,
                Name = command.Name,
            };
            await dataService.InsertAsync(transaction, cancellationToken);
            await context.SaveChanges(cancellationToken);
            return new Result
            {
                Id = transaction.Id,
            };
        }
    }
}