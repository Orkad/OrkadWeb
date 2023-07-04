using NHibernate.Event;
using NHibernate.Event.Default;
using OrkadWeb.Application.Users;
using OrkadWeb.Infrastructure.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Persistence.Listeners
{
    public class OwnableListener : IPreUpdateEventListener
    {
        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            var user = ServiceLocator.ServiceProvider.GetService<IAppUser>();
            return Task.FromResult(true);
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var user = ServiceLocator.ServiceProvider.GetService<IAppUser>();
            return true;

        }
    }
}
