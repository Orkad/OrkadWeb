using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Common.Interfaces
{
    public interface IDataContext
    {
        public Task BeginTransaction(CancellationToken cancellationToken);
        public Task Commit(CancellationToken cancellationToken);
    }
}
