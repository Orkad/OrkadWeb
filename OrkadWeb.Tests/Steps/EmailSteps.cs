using NFluent;
using OrkadWeb.Tests.Contexts;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class EmailSteps
    {
        private readonly EmailContext emailContext;
        public EmailContext.SendedEmail? LastSended { get; private set; }

        public EmailSteps(EmailContext emailContext)
        {
            this.emailContext = emailContext;
        }

        [Then(@"un email a bien été envoyé à l'adresse (.*)")]
        public void ThenUnEmailABienEteEnvoyeALadresse(string excepted)
        {
            Check.That(emailContext.SendedEmails).Not.IsEmpty();
            LastSended = emailContext.SendedEmails.Find(e => e.To == excepted);
            Check.That(LastSended).IsNotNull();
        }

    }
}
