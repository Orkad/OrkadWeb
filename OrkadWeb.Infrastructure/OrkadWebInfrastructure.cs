using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OrkadWeb.Tests")]

namespace OrkadWeb.Infrastructure
{
    public static class OrkadWebInfrastructure
    {
        public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    }
}
