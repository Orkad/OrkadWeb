using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using System.Reflection;

namespace OrkadWeb.Domain.NHibernate
{
    public static class OrkadWebConfigurationBuilder
    {
        public static Configuration Build(IPersistenceConfigurer persistenceConfigurer)
        {
            return Fluently.Configure()
                    .Database(persistenceConfigurer)
                    .Mappings(m => m
                        .FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();
        }
    }
}
