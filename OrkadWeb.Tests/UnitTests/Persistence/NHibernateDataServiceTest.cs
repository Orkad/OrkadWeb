using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Tool.hbm2ddl;
using OrkadWeb.Domain.Exceptions;
using OrkadWeb.Infrastructure.Persistence;
using System.Threading;

namespace OrkadWeb.Tests.UnitTests.Persistence
{
    [TestClass]
    public class NHibernateDataServiceTest
    {
        private class TestEntity
        {
            public virtual int Id { get; init; }
            public virtual string Name { get; init; }
        }

        private class TestEntityMap : ClassMap<TestEntity>
        {
            public TestEntityMap()
            {
                Id(x => x.Id).GeneratedBy.Identity();
                Map(x => x.Name);
            }
        }

        [TestMethod]
        public async Task TransactTest()
        {
            var configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory())
                .Mappings(mapping => mapping.FluentMappings.Add<TestEntityMap>())
                .BuildConfiguration();
            var sessionFactory = configuration.BuildSessionFactory();
            var session = sessionFactory.OpenSession();
            new SchemaExport(configuration)
                .Execute(false, true, false, session.Connection, null);
            var service = new NHibernateDataService(session);
            try
            {
                await service.TransactAsync(async () =>
                {
                    await service.InsertAsync(new TestEntity
                    {
                        Name = "Test",
                    });
                    var entity = service.Get<TestEntity>(1);
                    Check.That(entity.Name).IsEqualTo("Test");
                    throw new System.Exception("FAIL");
                });
            }
            catch
            {

            }
            Check.ThatCode(async () =>
            {
                await service.GetAsync<TestEntity>(1);
            }).Throws<DataNotFoundException<TestEntity>>();
        }
    }
}
