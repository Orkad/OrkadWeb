using MediatR;
using NFluent;
using OrkadWeb.Application.Users.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OrkadWeb.Angular.Controllers;
using TechTalk.SpecFlow;

namespace OrkadWeb.Specs.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly AuthController authController;
        private LoginCommand.Result response;

        public LoginSteps(AuthController authController)
        {
            this.authController = authController;
        }

        [When(@"je me connecte avec la combinaison (.*) / (.*)")]
        public async Task WhenJeMeConnecteAvecLaCombinaisonTestOkpassword(string username, string password)
        {
            response = await authController.Login(new LoginCommand
            {
                Username = username,
                Password = password
            }, CancellationToken.None);
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
