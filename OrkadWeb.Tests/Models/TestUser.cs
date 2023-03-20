using OrkadWeb.Application.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Tests.Models
{
    class TestUser : IAppUser
    {
        private readonly User user;

        public TestUser(User user)
        {
            this.user = user;
        }

        public int Id => user.Id;

        public string Name => user.Username;

        public string Email => user.Email;

        public string Role => user.Role;
    }
}
