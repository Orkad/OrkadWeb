using NHibernate.Bytecode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Persistence
{
    public class DependencyInjectionObjectFactory : IObjectsFactory
    {
        private readonly IServiceProvider serviceProvider;

        public DependencyInjectionObjectFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object CreateInstance(Type type)
        {
            return serviceProvider.GetService(type) ?? Activator.CreateInstance(type);
        }

        public object CreateInstance(Type type, bool nonPublic)
        {
            return serviceProvider.GetService(type);
        }

        public object CreateInstance(Type type, params object[] ctorArgs)
        {
            return Activator.CreateInstance(type, ctorArgs);
        }
    }
}
