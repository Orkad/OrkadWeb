using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationExtensions
    {
        public static string GetRequiredValue(this IConfiguration configuration, string key, int minLength = 1)
        {
            var value = configuration.GetRequiredSection(key).Value;
            if (value.Length < minLength)
            {
                throw new System.InvalidOperationException($"The value provided by key '{key}' does not match the minimum length requirement ({minLength})");
            }
            return value;
        }
    }
}
