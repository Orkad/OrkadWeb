using FluentValidation;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Logic.Abstractions;
using OrkadWeb.Logic.CQRS;
using OrkadWeb.Logic.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Expenses.Commands
{
    /// <summary>
    /// Permet d'ajouter une dépense
    /// </summary>
    public class AddExpenseCommand : ICommand<AddExpenseCommand.Result>
    {
        /// <summary>
        /// Montant de la dépense
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Nom d'affichage de la dépense
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date de la dépense (optionnel)
        /// </summary>
        public DateTime? Date { get; set; }

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

        public class AddExpenseValidator : AbstractValidator<AddExpenseCommand>
        {
            public AddExpenseValidator(ITimeProvider timeProvider)
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

        public class Handler : ICommandHandler<AddExpenseCommand, Result>
        {
            private readonly IDataService dataService;
            private readonly IAuthenticatedUser authenticatedUser;

            public Handler(IDataService dataService, IAuthenticatedUser authenticatedUser)
            {
                this.dataService = dataService;
                this.authenticatedUser = authenticatedUser;
            }

            public async Task<Result> Handle(AddExpenseCommand command, CancellationToken cancellationToken)
            {
                var transaction = new Transaction
                {
                    Amount = command.Amount,
                    Date = command.Date ?? DateTime.Now,
                    Name = command.Name,
                    Owner = dataService.Load<User>(authenticatedUser.Id),
                };
                await dataService.InsertAsync(transaction);
                return new Result
                {
                    Id = transaction.Id,
                };
            }
        }
    }
}
