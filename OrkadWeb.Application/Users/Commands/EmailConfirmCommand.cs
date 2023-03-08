using FluentValidation;
using NHibernate.Linq;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Common;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Domain.Utils;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GlobalConfiguration = OrkadWeb.Application.Config.GlobalConfiguration;

namespace OrkadWeb.Application.Users.Commands
{
    public class EmailConfirmCommand : ICommand
    {
        /// <summary>
        /// Email to confirm
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Confirmation hash
        /// </summary>
        public string ConfirmationHash { get; set; }

        class Validator : AbstractValidator<EmailConfirmCommand>
        {
            public Validator()
            {
                RuleFor(command => command.Email).NotEmpty().Matches(GlobalConfiguration.EMAIL_REGEX);
                RuleFor(command => command.ConfirmationHash).NotEmpty().Length(GlobalConfiguration.EMAIL_CONFIRMATION_HASH_LENGHT);
            }
        }

        class Handler : ICommandHandler<EmailConfirmCommand>
        {
            private readonly IDataService dataService;
            private readonly ITimeProvider timeProvider;

            public Handler(IDataService dataService, ITimeProvider timeProvider)
            {
                this.dataService = dataService;
                this.timeProvider = timeProvider;
            }

            public async Task<Unit> Handle(EmailConfirmCommand command, CancellationToken cancellationToken)
            {
                var user = await dataService.FindAsync<User>(u => u.Email == command.Email, cancellationToken);
                if (user == null)
                {
                    throw new EmailConfirmationException("user was not found");
                }
                if (user.Confirmation != null)
                {
                    throw new EmailConfirmationException("user already confirmed email");
                }
                if (!Hash.Validate(user.Email, command.ConfirmationHash))
                {
                    throw new EmailConfirmationException("wrong confirmation hash");
                }
                user.Confirmation = timeProvider.Now;
                return Unit.Value;
            }
        }


        [System.Serializable]
        public class EmailConfirmationException : System.Exception
        {
            public EmailConfirmationException() { }
            public EmailConfirmationException(string message) : base(message) { }
            public EmailConfirmationException(string message, System.Exception inner) : base(message, inner) { }
            protected EmailConfirmationException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}
