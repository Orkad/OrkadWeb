using OrkadWeb.Application.Users.Queries;
using System.Collections.Generic;
using System.Net.Http;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class AdminSteps
    {
        private readonly IMediator mediator;
        private List<GetAllUsersQuery.Result>? results;

        public AdminSteps(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [When(@"j'affiche la liste des utilisateurs")]
        public async Task WhenJafficheLaListeDesUtilisateurs()
        {
            results = await mediator.Send(new GetAllUsersQuery());
        }

        [Then(@"il y a les utilisateurs suivants")]
        public void ThenIlYALesUtilisateursSuivants(Table table)
        {
            Assert.IsNotNull(results);
            var expecteds = table.CreateSet<GetAllUsersQuery.Result>();
            foreach (var expected in expecteds)
            {
                var result = results.Single(result => result.Name == expected.Name);
                Check.That(result.Role).IsEqualIgnoringCase(expected.Role);
            }
        }

    }
}
