using OrkadWeb.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Contexts
{
    [Binding]
    public class JobRunnerContext : IJobRunner
    {
        public readonly List<Expression<Action>> Jobs = new List<Expression<Action>>();
        private int jobId = 1;
        public string Run(Expression<Action> action)
        {
            Jobs.Add(action);
            return jobId++.ToString();
        }
    }
}
