using MediatR;
using Microsoft.AspNetCore.SignalR;
using OrkadWeb.Application.Users.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Hubs
{
    public class NotificationHub : Hub<INotificationHubClient>
    {

    }

    public interface INotificationHubClient
    {
        public Task UserLoggedIn(string username);
    }

    public class Handler : INotificationHandler<UserLoggedInNotification>
    {
        private readonly IHubContext<NotificationHub, INotificationHubClient> context;

        public Handler(IHubContext<NotificationHub, INotificationHubClient> context)
        {
            this.context = context;
        }

        public async Task Handle(UserLoggedInNotification notification, CancellationToken cancellationToken)
        {
            await context.Clients.All.UserLoggedIn(notification.UserName);
        }
    }
}
