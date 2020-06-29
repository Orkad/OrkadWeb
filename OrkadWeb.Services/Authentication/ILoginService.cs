using System.Security.Principal;

namespace OrkadWeb.Services.Authentication
{
    public interface ILoginService
    {
        LoginResult Login(LoginCredentials credentials);
    }
}