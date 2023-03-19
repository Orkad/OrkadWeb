using OrkadWeb.Application.Users.Commands;

namespace OrkadWeb.Application.Users.Notifications
{
    public class UserRegisteredNotification : INotification
    {
        public string UserName { get; set; }

        public class Handler : INotificationHandler<UserRegisteredNotification>
        {
            private readonly IJobRunner jobRunner;
            private readonly ISender sender;

            public Handler(IJobRunner jobRunner, ISender sender)
            {
                this.jobRunner = jobRunner;
                this.sender = sender;
            }
            public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
            {
                jobRunner.Run(() => sender.Send(new SendEmailConfirmCommand
                {
                    Username = notification.UserName,
                }, cancellationToken));
            }
        }
    }
}
