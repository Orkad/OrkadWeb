using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using OrkadWeb.Common;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Users.Commands
{
    public class RegisterCommand : IRequest
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

        public class RegisterValidator : AbstractValidator<RegisterCommand>
        {
            public static Regex EmailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            public static Regex PasswordRegex = new Regex(@"(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[*.!@$%^&(){}[]:;<>,.?/~_+-=|\]).{8,32}$");

            public RegisterValidator(IDataService dataService)
            {
                CascadeMode = CascadeMode.Stop;
                RuleFor(command => command.UserName)
                    .NotEmpty().WithMessage("username is required")
                    .Length(5, 32).WithMessage("username must be at least 5 characters and 32 max");
                RuleFor(command => command.Email)
                    .NotEmpty().WithMessage("email is required")
                    .Matches(EmailRegex).WithMessage("email is incorrect")
                    .Must(email => !dataService.Query<User>().Any(u => u.Email == email)).WithMessage("email is already used by someone else");
                RuleFor(command => command.Password)
                    .NotEmpty().WithMessage("password is required")
                    .Matches(PasswordRegex).WithMessage($"password must be at least 8 characters, one lower, one upper, and one special character");
            }
        }

        public class RegisterHandler : IRequestHandler<RegisterCommand>
        {
            private readonly IDataService dataService;

            public RegisterHandler(IDataService dataService)
            {
                this.dataService = dataService;
            }
            public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                await dataService.InsertAsync(new User
                {
                    Email = request.Email,
                    Username = request.Email,
                    Password = Hash.Create(request.Password),
                });
                return Unit.Value;
            }
        }
    }
}
