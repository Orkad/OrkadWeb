using Microsoft.Extensions.Configuration;

namespace OrkadWeb.Domain.Extensions
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

        public static int GetRequiredIntValue(this IConfiguration configuration, string key)
        {
            var value = configuration.GetRequiredSection(key).Value;
            if (!int.TryParse(value, out var intValue))
            {
                throw new System.InvalidCastException($"The value provided by key '{key}' is not a number");
            }
            return intValue;
        }
    }
}
