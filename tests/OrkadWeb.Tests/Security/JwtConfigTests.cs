using Microsoft.Extensions.Configuration;
using OrkadWeb.Angular.Config;
using OrkadWeb.Infrastructure.Extensions;

namespace OrkadWeb.Tests.Security
{
    [TestClass]
    public class JwtConfigTests
    {
        [TestMethod]
        public void SecurityKeyMinLenghtTest()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", new string('A', 32)}, // 32 lenght
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            Check.That(new JwtConfig(configuration).SecurityKey).IsNotNull();
        }

        [TestMethod]
        public void SecurityKeyShouldRaise()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", new string('A', 31)}, // 21 lenght
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            Check.ThatCode(() => new JwtConfig(configuration).SecurityKey).Throws<InvalidOperationException>();
        }
    }
}
