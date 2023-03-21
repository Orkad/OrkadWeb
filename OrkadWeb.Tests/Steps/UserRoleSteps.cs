
namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class UserRoleSteps
    {
        private readonly IRepository dataService;
        private readonly LastContext lastContext;

        public UserRoleSteps(IRepository dataService, LastContext lastContext)
        {
            this.dataService = dataService;
            this.lastContext = lastContext;
        }

        [Given(@"l'utilisateur est administrateur")]
        public void GivenLutilisateurEstAdministrateur()
        {
            var user = lastContext.Last<User>();
            user.Role = UserRoles.Admin;
            dataService.Update(user);
        }
    }
}
