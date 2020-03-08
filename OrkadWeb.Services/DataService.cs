using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrkadWeb.Services
{
    public class DataService : IDataService
    {
        private readonly ISession session;

        public DataService(ISession session)
        {
            this.session = session;
        }

        public IQueryable<T> Query<T>() => session.Query<T>();
    }
}
