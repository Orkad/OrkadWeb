using OrkadWeb.Logic.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Tests.Models
{
    class TestUser : IAuthenticatedUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
