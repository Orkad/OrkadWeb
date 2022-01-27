using FluentValidation;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Abstractions;
using System;
using System.Linq;

namespace OrkadWeb.Logic.Shares.Commands.AddExpenseOnShare
{
    public class AddExpenseValidator : AbstractValidator<AddExpenseCommand>
    {
        private readonly IDataService dataService;
        private readonly IAuthenticatedUser authenticatedUser;

        public AddExpenseValidator(IDataService dataService, IAuthenticatedUser authenticatedUser, ITimeProvider timeProvider)
        {
            this.dataService = dataService;
            this.authenticatedUser = authenticatedUser;
            CascadeMode = CascadeMode.Stop;
            RuleFor(command => command.Amount).GreaterThan(0).WithMessage("Le montant doit être suppérieur à 0€");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Le nom de la dépense doit être défini");
            RuleFor(command => command.Date).LessThan(timeProvider.Now).When(command => command.Date.HasValue).WithMessage("La date doit être inférieure à la date du jour");

            RuleFor(command => command.ShareId)
                .Must(ShareMustExistsIfDefined).When(command => command.ShareId.HasValue).WithMessage("Le partage sur lequel ajouter la dépense n'existe pas")
                .Must(UserMustBeInShare).WithMessage("Vous n'ête pas autorisé à ajouter la dépense sur ce partage");
        }

        private bool UserMustBeInShare(int? shareId) => dataService.Get<Share>(shareId).UserShares.Any(us => us.User.Id == authenticatedUser.Id);

        private bool ShareMustExistsIfDefined(int? shareId) => dataService.Get<Share>(shareId) != null;
    }
}
