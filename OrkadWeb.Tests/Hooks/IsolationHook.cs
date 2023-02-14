using NHibernate;
using OrkadWeb.Application.Common.Interfaces;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Hooks
{
    [Binding]
    public class IsolationHook
    {
        [BeforeScenario]
        public void BeforeScenario(ISession session)
        {
            session.BeginTransaction();
        }

        [AfterScenario]
        public async Task AfterScenario(ISession session, IUnitOfWork unitOfWork)
        {
            await unitOfWork.CancelChangesAsync(default);
        }
    }
}
