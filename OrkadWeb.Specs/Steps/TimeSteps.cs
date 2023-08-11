using System;
using System.Collections.Generic;
using System.Text;
using OrkadWeb.Specs.Contexts;
using TechTalk.SpecFlow;

namespace OrkadWeb.Specs.Steps
{
    [Binding]
    public class TimeSteps
    {
        private readonly TimeContext timeContext;

        public TimeSteps(TimeContext timeContext)
        {
            this.timeContext = timeContext;
        }

        [Given(@"nous somme le (.*)")]
        public void GivenNousSommeLe(DateTime date)
        {
            timeContext.ChangeNow(date);
        }
    }
}
