using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using OrkadWeb.Common;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Config;
using OrkadWeb.Logic.CQRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Users.Commands
{
    public class RegisterCommand : ICommand
    {
        /// <summary>
        /// (required) username 5 to 32 characters
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// (required) valid email adress
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// (required) password with at least 8 characters, one lower, one upper, and one special character
        /// </summary>
        public string Password { get; set; }

        public class Validator : AbstractValidator<RegisterCommand>
        {
            private readonly IDataService dataService;

            public Validator(IDataService dataService)
            {
                this.dataService = dataService;
                ClassLevelCascadeMode = CascadeMode.Stop;

                RuleFor(command => command.UserName)
                    .NotEmpty().WithErrorCode(ErrorCodes.USERNAME_REQUIRED)
                    .MinimumLength(GlobalConfiguration.USERNAME_MIN_LENGHT).WithErrorCode(ErrorCodes.USERNAME_TOO_SHORT)
                    .MaximumLength(GlobalConfiguration.USERNAME_MAX_LENGHT).WithErrorCode(ErrorCodes.USERNAME_TOO_LONG)
                    .Matches(GlobalConfiguration.USERNAME_REGEX).WithErrorCode(ErrorCodes.USERNAME_WRONG_FORMAT)
                    .Must(NotMatchAnotherUsername).WithErrorCode(ErrorCodes.USERNAME_ALREADY_EXISTS);

                RuleFor(command => command.Email)
                    .NotEmpty().WithErrorCode(ErrorCodes.EMAIL_REQUIRED)
                    .Matches(GlobalConfiguration.EMAIL_REGEX).WithErrorCode(ErrorCodes.EMAIL_WRONG_FORMAT)
                    .Must(NotMatchAnotherEmail).WithErrorCode(ErrorCodes.EMAIL_ALREADY_EXISTS);

                RuleFor(command => command.Password)
                    .NotEmpty().WithErrorCode("obligatoire")
                    .MinimumLength(GlobalConfiguration.PASSWORD_MIN_LENGHT).WithErrorCode(ErrorCodes.PASSWORD_TOO_SHORT)
                    .MaximumLength(GlobalConfiguration.PASSWORD_MAX_LENGHT).WithErrorCode(ErrorCodes.PASSWORD_TOO_LONG)
                    .Matches(GlobalConfiguration.PASSWORD_REGEX).WithErrorCode(ErrorCodes.PASSWORD_WRONG_FORMAT);
            }

            private bool NotMatchAnotherUsername(string username) => dataService.NotExists<User>(u => u.Username == username);

            private bool NotMatchAnotherEmail(string email) => dataService.NotExists<User>(u => u.Email == email);
        }

        public class Handler : ICommandHandler<RegisterCommand>
        {
            private readonly IDataService dataService;

            public Handler(IDataService dataService)
            {
                this.dataService = dataService;
            }
            public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                await dataService.InsertAsync(new User
                {
                    Email = request.Email,
                    Username = request.UserName,
                    Password = Hash.Create(request.Password),
                    Creation = DateTime.Now,
                });
                return Unit.Value;
            }
        }
    }
}
