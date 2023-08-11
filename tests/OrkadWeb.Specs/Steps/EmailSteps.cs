using NFluent;
using OrkadWeb.Specs.Contexts;
using OrkadWeb.Specs.Models;
using TechTalk.SpecFlow;

namespace OrkadWeb.Specs.Steps
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
