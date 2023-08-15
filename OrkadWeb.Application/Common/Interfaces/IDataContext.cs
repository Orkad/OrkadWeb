using System;

namespace OrkadWeb.Application.Common.Interfaces;

public interface IDataContext : IDisposable
{
    public Task SaveChanges(CancellationToken cancellationToken);
}