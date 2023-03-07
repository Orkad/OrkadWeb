using System;
using System.Linq.Expressions;

namespace OrkadWeb.Application.Common.Interfaces
{
    public interface IJobRunner
    {
        string Run(Expression<Action> action);
    }
}
