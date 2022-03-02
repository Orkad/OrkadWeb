using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Data.Builder;
using OrkadWeb.Data.NHibernate;
using OrkadWeb.Logic;
using OrkadWeb.Logic.Abstractions;
using OrkadWeb.Tests.Contexts;
using OrkadWeb.Tests.Models;
using SolidToken.SpecFlow.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OrkadWeb.Tests.Support
{
    public static class ServiceCollectionBuilder
    {
        [ScenarioDependencies]
        public static IServiceCollection BuildServiceCollection()
        {
            var services = new ServiceCollection();

            var resolver = new InMemorySessionFactoryResolver();
            var sessionFactory = resolver.Resolve(Assembly.LoadFrom("OrkadWeb.Data"));
            services.AddSingleton<ISessionFactoryResolver>(resolver);
            services.AddSingleton(sessionFactory);
            services.AddData();
            services.AddLogic();
            services.AddSingleton<ITimeProvider, TimeContext>();

            return services;
        }
    }
}
