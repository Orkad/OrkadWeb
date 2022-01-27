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
using System.Text;

namespace OrkadWeb.Tests.Support
{
    public static class ServiceCollectionBuilder
    {
        [ScenarioDependencies]
        public static IServiceCollection BuildServiceCollection()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ISessionFactoryResolver, InMemorySessionFactoryResolver>();
            services.AddData();
            services.AddLogic();
            services.AddSingleton<ITimeProvider, TimeContext>();

            return services;
        }
    }
}
