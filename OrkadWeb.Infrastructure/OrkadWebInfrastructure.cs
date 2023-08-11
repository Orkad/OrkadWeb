using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OrkadWeb.Specs")]
[assembly: InternalsVisibleTo("OrkadWeb.Tests")]

namespace OrkadWeb.Infrastructure
{
    public class OrkadWebInfrastructure
    {
        public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    }
}
