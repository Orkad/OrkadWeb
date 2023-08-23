using OrkadWeb.Application.Users;
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping;

namespace OrkadWeb.Specs.Models
{
    class TestUser : IAppUser
    {
        private User _user;

        public void Set(User user)
        {
            this._user = user;
        }

        public int Id => _user.Id;

        public string Name => _user.Username;

        public string Email => _user.Email;

        public string Role => _user.Role;
    }
}
