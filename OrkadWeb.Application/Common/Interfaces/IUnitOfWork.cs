using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task SaveChangesAsync(CancellationToken cancellationToken);
        public Task CancelChangesAsync(CancellationToken cancellationToken);
    }
}
