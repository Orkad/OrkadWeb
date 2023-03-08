using NFluent;
using OrkadWeb.Tests.Contexts;
using OrkadWeb.Tests.Models;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class EmailSteps
    {
        private readonly LastContext lastContext;

        public EmailSteps(LastContext lastContext)
        {
            this.lastContext = lastContext;
        }

        [Then(@"un email a bien été envoyé à l'adresse (.*)")]
        public void ThenUnEmailABienEteEnvoyeALadresse(string excepted)
        {
            var email = lastContext.Last<SendedEmail>();
            Check.That(email).IsNotNull();
            Check.That(email.To).IsEqualTo(excepted);
        }

    }
}
