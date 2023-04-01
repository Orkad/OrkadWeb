using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrkadWeb.Application.Users;
using OrkadWeb.Application.Users.Commands;
using OrkadWeb.Application.Users.Notifications;

namespace OrkadWeb.Angular.Controllers;

[ApiController]
[Authorize]
[Route("api/auth/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAppUser appUser;
    private readonly IPublisher publisher;
    private readonly ISender sender;

    public AuthController(IAppUser appUser, ISender sender, IPublisher publisher)
    {
        this.appUser = appUser;
        this.sender = sender;
        this.publisher = publisher;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<LoginCommand.Result> Login(LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        await publisher.Publish(new UserLoggedInNotification
        {
            UserName = command.Username
        }, cancellationToken);
        return result;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task Register(RegisterCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task Confirm(EmailConfirmCommand command, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
    }

    [HttpPost]
    public async Task ResendConfirm(CancellationToken cancellationToken)
    {
        await sender.Send(new SendEmailConfirmCommand
        {
            Username = appUser.Name
        }, cancellationToken);
    }
}