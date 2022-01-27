using OrkadWeb.Logic.Abstractions;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Tests.Contexts
{
    public class UserContext
    {
        public IAuthenticatedUser AuthenticatedUser { get; set; }
    }
}
