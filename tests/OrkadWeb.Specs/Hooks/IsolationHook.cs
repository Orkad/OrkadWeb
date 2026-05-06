using NHibernate;

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
            await transaction.RollbackAsync();
        }
    }
}
