using OrkadWeb.Application.Security;

namespace OrkadWeb.Tests.Common
{
    [TestClass]
    public class HashTest
    {
        /// <summary>
        /// sha256 of "test"
        /// </summary>
        private const string TEST_SHA256 = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08";

        [TestMethod]
        public void Sha256Test()
        {
            Check.That(Hash.Sha256("test")).IsEqualTo(TEST_SHA256);
            Check.That(Hash.Sha256("test")).CountIs(Hash.SHA256_LENGTH);
        }

        [TestMethod]
        public void SaltTest()
        {
            var salts = Enumerable.Range(1, 100).Select(i => Hash.Salt()).ToArray();

            // all salt are differents
            Check.That(salts).ContainsNoDuplicateItem();

            // all salts are the same size (size defined by the class itself)
            Check.That(salts).ContainsOnlyElementsThatMatch(salt => salt.Length == Hash.SALT_LENGTH);

            // all salts are lowercases
            Check.That(salts).ContainsOnlyElementsThatMatch(salt => salt.All(c => char.IsLower(c) || char.IsDigit(c)));
        }

        [TestMethod]
        public void CreateTest()
        {
            var hashes = Enumerable.Range(1, 100).Select(i => Hash.Create("test")).ToArray();

            // all hashes are differents
            Check.That(hashes).ContainsNoDuplicateItem();

            // does not contains default esay findable sha256
            Check.That(hashes).Not.Contains(TEST_SHA256);

            // are the same size (size defined by the class itself)
            Check.That(hashes).ContainsOnlyElementsThatMatch(salt => salt.Length == Hash.SALT_LENGTH + Hash.SHA256_LENGTH);
        }

        [TestMethod]
        public void ValidateTest()
        {
            Check.That(Hash.Validate("test", Hash.Create("test"))).IsTrue();
            Check.That(Hash.Validate("TEST", Hash.Create("test"))).IsFalse();
            Check.That(Hash.Validate("test", null)).IsFalse();
            Check.That(Hash.Validate(null, "test")).IsFalse();
        }
    }
}
