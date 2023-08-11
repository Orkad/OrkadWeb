
using OrkadWeb.Specs.Contexts;

namespace OrkadWeb.Specs.Steps
{
    [Binding]
    public class UserRoleSteps
    {
        private readonly IDataService dataService;
        private readonly LastContext lastContext;

        public UserRoleSteps(IDataService dataService, LastContext lastContext)
        {
            this.dataService = dataService;
            this.lastContext = lastContext;
        }

        [Given(@"l'utilisateur est administrateur")]
        public void GivenLutilisateurEstAdministrateur()
        {
            var user = lastContext.Last<User>();
            user.Role = UserRoles.Admin;
        }
    }
}
