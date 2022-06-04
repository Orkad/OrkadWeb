using MediatR;
using MimeKit;
using OrkadWeb.Common;
using OrkadWeb.Logic.Abstractions;
using OrkadWeb.Logic.CQRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Users.Commands
{
    /// <summary>
    /// Send the confirmation email for validate an acount
    /// </summary>
    public class SendEmailConfirmationCommand : ICommand
    {

    }

    
    public class SendEmailConfirmationCommandHandler : ICommandHandler<SendEmailConfirmationCommand>
    {
        private readonly IAuthenticatedUser user;
        private readonly IEmailService emailService;

        public SendEmailConfirmationCommandHandler(IAuthenticatedUser user, IEmailService emailService)
        {
            this.user = user;
            this.emailService = emailService;
        }

        public async Task<Unit> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            var hash = 1234;
            var message = $@"Hello {user.Name},

You just register using this email adress.
Please follow the link to validate your inscription : {hash}
";
            await emailService.SendAsync(user.Email, "Confirm your email adress", message, cancellationToken);

            return Unit.Value;
        }
    }
}
