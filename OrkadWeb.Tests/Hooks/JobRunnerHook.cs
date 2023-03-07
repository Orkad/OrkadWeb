using OrkadWeb.Tests.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Hooks
{
    [Binding]
    public class JobRunnerHook
    {
        [AfterStep]
        public void AfterStep(JobRunnerContext jobRunnerContext)
        {
            foreach (var job in jobRunnerContext.Jobs)
            {
                job.Compile()();
            }
            jobRunnerContext.Jobs.Clear();
        }
    }
}
