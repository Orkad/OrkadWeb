using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using OrkadWeb.Data.NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OrkadWeb.Tests.Support
{
    public class InMemorySessionFactoryResolver : ISessionFactoryResolver, IDisposable
    {
        private IDbConnection? Connection;
        public ISessionFactory Resolve(Assembly assembly)
        {
            var config = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString("FullUri=file:memorydb.db?mode=memory&cache=shared"))
                    .Mappings(m => m.FluentMappings.AddFromAssembly(assembly)
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();
            var sessionFactory = config.BuildSessionFactory();
            Connection = sessionFactory.OpenSession().Connection;
            new SchemaExport(config).Execute(false, true, false);

            return sessionFactory;
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
