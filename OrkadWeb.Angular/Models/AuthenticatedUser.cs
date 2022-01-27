using OrkadWeb.Logic.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrkadWeb.Angular.Models
{
    /// <summary>
    /// Représente un utilisateur connecté sur l'api
    /// </summary>
    class AuthenticatedUser : IAuthenticatedUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
