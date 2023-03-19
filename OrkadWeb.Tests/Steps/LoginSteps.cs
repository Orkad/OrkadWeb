using MediatR;
using NFluent;
using OrkadWeb.Application.Users.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly IMediator mediator;
        private LoginCommand.Result response;

        public LoginSteps(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [When(@"je me connecte avec la combinaison (.*) / (.*)")]
        public async Task WhenJeMeConnecteAvecLaCombinaisonTestOkpassword(string username, string password)
        {
            var command = new LoginCommand
            {
                Username = username,
                Password = password
            };
            response = await mediator.Send(command);
        }

        [Then(@"la connexion a réussie")]
        public void ThenSuccessConnect()
        {
            Check.That(response.Success).IsTrue();
        }

        [Then(@"la connexion a échouée")]
        public void ThenFailConnect()
        {
            Check.That(response.Success).IsFalse();
        }
    }
}
