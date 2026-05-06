using OrkadWeb.Application.Users;
using OrkadWeb.Specs.Models;

namespace OrkadWeb.Specs.Contexts
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
