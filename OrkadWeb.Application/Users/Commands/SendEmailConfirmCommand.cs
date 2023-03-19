namespace OrkadWeb.Application.Users.Commands;

public class SendEmailConfirmCommand : ICommand
{
    public string Username { get; set; }

    class Validator : AbstractValidator<SendEmailConfirmCommand>
    {
        public Validator()
        {
            RuleFor(v => v.Username).NotEmpty();
        }
    }

    class Handler : ICommandHandler<SendEmailConfirmCommand>
    {
        private readonly IDataService dataService;
        private readonly IEmailService emailService;

        public Handler(IDataService dataService, IEmailService emailService)
        {
            this.dataService = dataService;
            this.emailService = emailService;
        }

        public async Task<Unit> Handle(SendEmailConfirmCommand command, CancellationToken cancellationToken)
        {
            var user = await dataService.FindAsync<User>(u => u.Username == command.Username, cancellationToken);
            var hash = Hash.Create(user.Email);
            var message = $@"Hello {user.Username},

You just register using this email adress.
Please follow the link to validate your email : 
<a href=""http://orkad.fr/auth/confirm?email={user.Email}&hash={hash}"">confirm your email</a>
";
            await emailService.SendAsync(user.Email, "Confirm your email adress", message, cancellationToken);
            return Unit.Value;
        }
    }
}
