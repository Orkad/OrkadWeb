using NHibernate;
using OrkadWeb.Application.Common.Interfaces;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Specs.Hooks
{
    [Binding]
    public class IsolationHook
    {
        private ITransaction transaction;

        [BeforeScenario]
        public void BeforeScenario(ISession session)
        {
            transaction = session.BeginTransaction();
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            if (transaction == null)
            {
                return;
            }
            await transaction.RollbackAsync();
        }
    }
}
