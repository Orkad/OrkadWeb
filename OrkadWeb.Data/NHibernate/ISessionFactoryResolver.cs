using NHibernate;
using System.Reflection;

namespace OrkadWeb.Data.NHibernate
{
    public interface ISessionFactoryResolver
    {
        /// <summary>
        /// Résolution de la session factory NHibernate basé sur une assembly
        /// </summary>
        /// <param name="assembly">Assembly contenant le mapping NHibernate</param>
        public ISessionFactory Resolve(Assembly assembly);
    }
}
