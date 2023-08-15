using OrkadWeb.Application.Users.Commands;

namespace OrkadWeb.Application.Users.Notifications;

public class UserRegisteredNotification : INotification
{
    public string UserName { get; init; }

    public class Handler : INotificationHandler<UserRegisteredNotification>
    {
        private readonly ISender sender;

        public Handler(ISender sender)
        {
            this.sender = sender;
        }
        public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            var command = new SendEmailConfirmCommand
            {
                Username = notification.UserName
            };
            await sender.Send(command, cancellationToken);
        }
    }
}