using OrkadWeb.Logic.Abstractions;
using System;

namespace OrkadWeb.Tests.Contexts
{
    public class TimeContext : ITimeProvider
    {
        public DateTime Now { get; private set; } = DateTime.Now;

        public void ChangeNow(DateTime date)
        {
            Now = date;
        }

    }
}
