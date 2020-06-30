using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;
using OrkadWeb.Models;
using OrkadWeb.Services;
using OrkadWeb.Services.Authentication;
using OrkadWeb.Services.Data;

namespace OrkadWebVue.Services
{
    public class LoginService : ILoginService
    {
        private readonly IDataService dataService;

        public LoginService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        private IQueryable<User> GetLoginQuery(LoginCredentials credentials)
        {
            var hash = HashUtils.HashSHA256(credentials.Password);
            return dataService.Query<User>().Where(u => (u.Username == credentials.Username || u.Email == credentials.Username) && u.Password == hash);
        }

        public bool ValidateLogin(LoginCredentials credentials)
            => GetLoginQuery(credentials).Any();

        public IPrincipal GetPrincipal(LoginCredentials credentials)
        {
            var user = GetLoginQuery(credentials).Single();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User"),
            };
            return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
        }
    }
}