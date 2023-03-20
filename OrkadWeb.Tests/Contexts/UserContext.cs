using OrkadWeb.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Contexts
{
    [Binding]
    public class UserContext
    {
        public IAppUser? AuthenticatedUser { get; set; }
    }
}
