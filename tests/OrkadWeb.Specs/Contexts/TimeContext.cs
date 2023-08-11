using OrkadWeb.Application.Common.Interfaces;
using System;

namespace OrkadWeb.Specs.Contexts
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
