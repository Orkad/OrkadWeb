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
        public string Hash { get; set; }

        class Handler : ICommandHandler<EmailConfirmCommand>
        {
            private readonly IRepository dataService;
            private readonly ITimeProvider timeProvider;

            public Handler(IRepository dataService, ITimeProvider timeProvider)
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
                if (!Domain.Utils.Hash.Validate(user.Email, command.Hash))
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
