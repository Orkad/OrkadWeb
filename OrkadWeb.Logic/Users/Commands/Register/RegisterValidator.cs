using FluentValidation;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace OrkadWeb.Logic.Users.Commands.Register
{
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
}
