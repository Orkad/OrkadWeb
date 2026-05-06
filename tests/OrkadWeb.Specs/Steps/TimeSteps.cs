using System;

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
