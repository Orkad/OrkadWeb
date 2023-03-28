using NHibernate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Common.Interfaces
{
    public interface IDataContext : IDisposable
    {
        public Task SaveChanges(CancellationToken cancellationToken);
    }
}
