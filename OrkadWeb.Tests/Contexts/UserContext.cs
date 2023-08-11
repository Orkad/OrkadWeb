using OrkadWeb.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrkadWeb.Tests.Models;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Contexts
{
    [Binding]
    public class UserContext
    {
        private TestUser authenticatedUser = new TestUser();
        public IAppUser AuthenticatedUser => authenticatedUser;

        public void SetAuthenticatedUser(User user)
        {
            authenticatedUser.Set(user);
        }
    }
}
