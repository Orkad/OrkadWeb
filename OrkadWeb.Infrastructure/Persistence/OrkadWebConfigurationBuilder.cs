using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using OrkadWeb.Infrastructure.Persistence.Conventions;
using System.Reflection;

namespace OrkadWeb.Infrastructure.Persistence
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
