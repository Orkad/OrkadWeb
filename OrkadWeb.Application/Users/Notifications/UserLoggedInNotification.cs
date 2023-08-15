namespace OrkadWeb.Application.Users.Notifications;

public class UserLoggedInNotification : INotification
{
    public string UserName { get; init; }
}