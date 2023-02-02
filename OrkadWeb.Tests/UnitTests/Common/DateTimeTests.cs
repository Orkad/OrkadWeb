using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Tests.UnitTests.Common
{
    [TestClass]
    public class DateTimeTests
    {
        [TestMethod]
        public void ToUnixTimestampTest()
        {
            Check.That(new DateTime(2000, 1, 2, 3, 4, 5).ToUnixTimestamp()).IsEqualTo(946778645);
            Check.That(new DateTime(2000, 1, 2, 3, 4, 6).ToUnixTimestamp()).IsEqualTo(946778646);
        }
    }
}
