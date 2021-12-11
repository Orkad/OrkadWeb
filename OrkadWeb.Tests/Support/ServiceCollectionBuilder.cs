using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Data.Builder;
using OrkadWeb.Data.NHibernate;
using OrkadWeb.Logic.Builder;
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

            return services;
        }
    }
}
