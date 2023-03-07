using Hangfire;
using OrkadWeb.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Jobs
{
    public class JobRunner : IJobRunner
    {
        private readonly IBackgroundJobClient backgroundJobClient;

        public JobRunner(IBackgroundJobClient backgroundJobClient)
        {
            this.backgroundJobClient = backgroundJobClient;
        }

        public string Run(Expression<Action> action)
        {
            return backgroundJobClient.Enqueue(action);
        }
    }
}
