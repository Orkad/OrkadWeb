using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using System.Reflection;

namespace OrkadWeb.Data.NHibernate
{
    public static class OrkadWebConfigurationBuilder
    {
        public static Configuration Build(string connectionString)
        {
            return Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m
                        .FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();
        }
    }
}
