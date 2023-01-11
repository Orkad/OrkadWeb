using Newtonsoft.Json;
using NHibernate.Cfg;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace OrkadWeb.Data.NHibernate
{
    /// <summary>
    /// Provide caching methods for NHibernate configuration
    /// </summary>
    internal class NHibernateConfigurationCache
    {
        /// <summary>
        /// assembly corresponding file
        /// </summary>
        private readonly FileInfo assemblyFile;
        /// <summary>
        /// assembly cache corresponding file
        /// </summary>
        private readonly FileInfo cacheFile;

        /// <summary>
        /// Provide caching methods for NHibernate configuration for this assembly
        /// </summary>
        /// <param name="assembly">given assembly (xml or fluent mapping inside)</param>
        public NHibernateConfigurationCache(Assembly assembly)
        {
            assemblyFile = new FileInfo(assembly.Location);
            var assemblyDirectory = Path.GetDirectoryName(assembly.Location);
            cacheFile = new FileInfo(Path.Combine(assemblyDirectory, assembly.GetName().Name + ".NH.cache"));
        }

        /// <summary>
        /// Determine if cache file is valid (existing & not expired based on file assembly creation date)
        /// </summary>
        private bool IsCacheFileValid()
        {
            if (!cacheFile.Exists || cacheFile.Length < 5 * 1024)
            {
                return false;
            }
            return cacheFile.LastWriteTime >= assemblyFile.LastWriteTime;
        }

        /// <summary>
        /// Save a NHibernate configuration to cache file "Assembly.Name.NH.cache"
        /// </summary>
        /// <param name="configuration">configuration to serialize</param>
        public void SaveConfigurationToFile(Configuration configuration)
        {
            using var file = cacheFile.Open(FileMode.Create);
            using var writer = new BinaryWriter(file);
            writer.Write(JsonConvert.SerializeObject(configuration));
        }

        /// <summary>
        /// Load an existing configuration from corresponding assembly cache file "Assembly.Name.NH.cache"<br/>
        /// returns null on invalid file
        /// </summary>
        public Configuration LoadConfigurationFromFile()
        {
            if (!IsCacheFileValid())
            {
                return null;
            }
            using var file = cacheFile.Open(FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(file);
            return JsonConvert.DeserializeObject<Configuration>(reader.ReadString());
        }
    }
}
