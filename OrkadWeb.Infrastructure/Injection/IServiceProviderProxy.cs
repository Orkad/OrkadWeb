using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Injection
{
    public interface IServiceProviderProxy
    {
        T GetService<T>();
        object GetService(Type type);
    }
}
