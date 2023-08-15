namespace OrkadWeb.Application.Users;

public interface IIdentityTokenGenerator
{
    /// <summary>
    /// Generate a identity token
    /// </summary>
    string GenerateToken(User user);
}