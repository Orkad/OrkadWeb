using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Injection
{
    public static class ServiceLocator
    {
        private static IServiceProviderProxy proxy;

        public static IServiceProviderProxy ServiceProvider => proxy ?? throw new Exception("You should Initialize the ServiceProvider before using it.");

        public static void Initialize(IServiceProviderProxy proxy)
        {
            ServiceLocator.proxy = proxy;
        }
    }
}
