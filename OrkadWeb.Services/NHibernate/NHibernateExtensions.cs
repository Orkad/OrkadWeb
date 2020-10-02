using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using OrkadWeb.Models.Enums.NHibernate;
using OrkadWeb.Services.NHibernate.Mapping;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace OrkadWeb.Services.NHibernate
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            // check cache
            var nhConfigCache = new NHibernateConfigurationCache(Assembly.GetAssembly(typeof(UserMap)));
            var config = nhConfigCache.LoadConfigurationFromFile(); // 175ms (si cache présent)
            if (config == null)
            {
                var fluentConfiguration = Fluently
                    .Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>().Conventions.Add<EnumConvention>()); // 132ms
                config = fluentConfiguration.BuildConfiguration(); // 549ms
                nhConfigCache.SaveConfigurationToFile(config); // 60ms
            }
            
            var sessionFactory = config.BuildSessionFactory(); // 539ms 

            services.AddSingleton(sessionFactory);
            services.AddScoped(serviceProvider => sessionFactory.OpenSession());

            return services;
        }
    }

    public class NHibernateConfigurationCache
    {
        private readonly string _cacheFile;
        private readonly Assembly _definitionsAssembly;

        public NHibernateConfigurationCache(Assembly definitionsAssembly)
        {
            _definitionsAssembly = definitionsAssembly;
            _cacheFile = "nh.cfg";
        }

        public void DeleteCacheFile()
        {
            if (File.Exists(_cacheFile))
                File.Delete(_cacheFile);
        }

        public bool IsConfigurationFileValid
        {
            get
            {
                if (!File.Exists(_cacheFile))
                    return false;
                var configInfo = new FileInfo(_cacheFile);
                var asmInfo = new FileInfo(_definitionsAssembly.Location);

                if (configInfo.Length < 5 * 1024)
                    return false;

                return configInfo.LastWriteTime >= asmInfo.LastWriteTime;
            }
        }

        public void SaveConfigurationToFile(Configuration configuration)
        {
            using (var file = File.Open(_cacheFile, FileMode.Create))
            {
                GetFormatter().Serialize(file, configuration);
            }
        }

        public Configuration LoadConfigurationFromFile()
        {
            if (!IsConfigurationFileValid)
                return null;

            using (var file = File.Open(_cacheFile, FileMode.Open, FileAccess.Read))
            {
                return GetFormatter().Deserialize(file) as Configuration;
            }
        }

        private IFormatter GetFormatter()
            => new BinaryFormatter
            {
                SurrogateSelector = new FluentNHibernate.Infrastructure.NetStandardSerialization.SurrogateSelector()
            };
    }
}
