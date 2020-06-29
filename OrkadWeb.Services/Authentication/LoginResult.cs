using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrkadWeb.Services.Authentication
{
    public class LoginResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
