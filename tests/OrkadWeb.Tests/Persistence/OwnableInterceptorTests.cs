using System.Security;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using OrkadWeb.Application.Users;
using OrkadWeb.Application.Users.Exceptions;
using OrkadWeb.Domain.Consts;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Domain.Primitives;
using OrkadWeb.Infrastructure;
using OrkadWeb.Infrastructure.Persistence.Interceptors;
using OrkadWeb.Infrastructure.Persistence.Mapping;

namespace OrkadWeb.Tests.Persistence;

[TestClass]
public class OwnableInterceptorTests
{
    private ISession selfSession;
    private ISession otherSession;
    private ISessionFactory sessionFactory;

    private class OwnableEntity : IOwnable
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual User Owner { get; }
    }

    private class OwnableEntityMap : ClassMap<OwnableEntity>
    {
        public OwnableEntityMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name);
            References(x => x.Owner);
        }
    }

    private class OtherUser : IAppUser
    {
        public int Id { get; } = 999;
        public string Name { get; } = "Other";
        public string Email { get; } = "other@email.com";
        public string Role { get; } = UserRoles.User;
    }

    private class SelfUser : IAppUser
    {
        public int Id { get; } = 1;
        public string Name { get; } = "Self";
        public string Email { get; } = "self@email.com";
        public string Role { get; } = UserRoles.User;
    }

    [TestInitialize]
    public void Initialize()
    {
        var configuration = Fluently.Configure()
            .Database(SQLiteConfiguration.Standard.InMemory())
            .Mappings(mapping => mapping.FluentMappings.Add<OwnableEntityMap>().Add<UserMap>())
            .BuildConfiguration();
        sessionFactory = configuration.BuildSessionFactory();
        var selfUser = new SelfUser();
        var otherUser = new OtherUser();
        selfSession = sessionFactory
            .WithOptions().Interceptor(new OwnableInterceptor(selfUser))
            .OpenSession();
        otherSession = sessionFactory
            .WithOptions().Interceptor(new OwnableInterceptor(otherUser)).Connection(selfSession.Connection)
            .OpenSession();
        new SchemaExport(configuration).Execute(false, true, false, selfSession.Connection, null);
        selfSession.Save(new User
        {
            Id = selfUser.Id,
            Username = selfUser.Name,
            Email = selfUser.Email,
            Role = selfUser.Role,
            Password = "self",
        });
        otherSession.Save(new User
        {
            Id = otherUser.Id,
            Username = otherUser.Name,
            Email = otherUser.Email,
            Role = otherUser.Role,
            Password = "other"
        });
    }

    [TestCleanup]
    public void Cleanup()
    {
        otherSession.Dispose();
        selfSession.Dispose();
        sessionFactory.Dispose();
    }

    [TestMethod]
    public void ShouldAutoAffectOwnerTest()
    {
        var ownedEntity = new OwnableEntity();
        selfSession.Save(ownedEntity);
        Check.That(ownedEntity.Owner).IsNotNull();
        Check.That(ownedEntity.Owner.Id).IsEqualTo(1);
    }

    [TestMethod]
    public void ShouldDenyUpdateFromOthersTest()
    {
        var ownedEntity = new OwnableEntity
        {
            Id = 1,
        };
        selfSession.Save(ownedEntity);
        selfSession.Flush();
        var notOwnedEntity = otherSession.Get<OwnableEntity>(1);
        notOwnedEntity.Name = "NamedChanged";
        Check.ThatCode(() => otherSession.Flush()).Throws<NotOwnedException>();
    }

    [TestMethod]
    public void ShouldDenyDeleteFromOthersTest()
    {
        var ownedEntity = new OwnableEntity
        {
            Id = 1,
        };
        selfSession.Save(ownedEntity);
        selfSession.Flush();
        var notOwnedEntity = otherSession.Get<OwnableEntity>(1);
        Check.ThatCode(() => otherSession.Delete(notOwnedEntity)).Throws<NotOwnedException>();
    }
}