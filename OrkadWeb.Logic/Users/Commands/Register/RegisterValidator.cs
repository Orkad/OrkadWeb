using FluentValidation;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Config;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace OrkadWeb.Logic.Users.Commands.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IDataService dataService;

        public RegisterValidator(IDataService dataService)
        {
            this.dataService = dataService;
            CascadeMode = CascadeMode.Stop;

            RuleFor(command => command.UserName)
                .NotEmpty().WithMessage("obligatoire")
                .MinimumLength(GlobalConfiguration.USERNAME_MIN_LENGHT).WithMessage("trop court")
                .MaximumLength(GlobalConfiguration.USERNAME_MAX_LENGHT).WithMessage("trop long")
                .Matches(GlobalConfiguration.USERNAME_REGEX).WithMessage("alphanumérique uniquement")
                // BACKEND CHECK ONLY
                .Must(NotMatchAnotherUsername).WithMessage("déjà utilisé");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("obligatoire")
                .Matches(GlobalConfiguration.EMAIL_REGEX).WithMessage("email invalide")
                // BACKEND CHECK ONLY
                .Must(NotMatchAnotherEmail).WithMessage("déjà utilisé");

            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("obligatoire")
                .MinimumLength(GlobalConfiguration.PASSWORD_MIN_LENGHT).WithMessage("trop court")
                .MaximumLength(GlobalConfiguration.PASSWORD_MAX_LENGHT).WithMessage("trop long")
                .Matches(GlobalConfiguration.PASSWORD_REGEX).WithMessage("au moins une majuscule, une minuscule et un caractère spécial");
            
        }

        private bool NotMatchAnotherUsername(string username) => dataService.NotExists<User>(u => u.Username == username);

        private bool NotMatchAnotherEmail(string email) => dataService.NotExists<User>(u => u.Email == email);
    }
}
