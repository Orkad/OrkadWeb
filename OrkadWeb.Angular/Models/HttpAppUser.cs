using Microsoft.AspNetCore.Http;
using OrkadWeb.Application.Users;
using System;
using System.Security.Claims;

namespace OrkadWeb.Angular.Models
{
    /// <summary>
    /// Représente un utilisateur connecté sur l'api
    /// </summary>
    public class HttpAppUser : IAppUser
    {
        public HttpAppUser(IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext ?? throw new ArgumentException("http context not available", nameof(httpContextAccessor));
            var user = context.User;
            if (user.Identity.IsAuthenticated)
            {
                Id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
                Name = user.FindFirst(ClaimTypes.Name).Value;
                Email = user.FindFirst(ClaimTypes.Email).Value;
                Role = user.FindFirst("role").Value;
            }
            else
            {
                Id = default;
                Name = "Anonymous";
                Email = "";
                Role = "None";
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
