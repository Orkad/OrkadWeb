using System;
using System.Collections.Generic;
using System.Linq;

namespace OrkadWeb.Services
{
    public static class Services
    {
        /// <summary>
        /// Récupère tous les types de services de cette assembly
        /// </summary>
        public static IEnumerable<Type> GetServiceTypes()
        {
            var iserviceType = typeof(IService);
            return iserviceType.Assembly.GetTypes().Where(t => iserviceType.IsAssignableFrom(t) && !t.IsAbstract);
        }
    }
}
