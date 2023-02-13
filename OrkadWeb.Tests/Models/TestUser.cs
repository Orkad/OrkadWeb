using OrkadWeb.Application.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Tests.Models
{
    class TestUser : IAuthenticatedUser
    {
        public TestUser(int id, string name, string email)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }


    }
}
