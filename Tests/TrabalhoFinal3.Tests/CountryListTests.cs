using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrabalhoFinal3.Data;

namespace TrabalhoFinal3.Tests
{
    [TestClass]
    public class CountryListTests
    {
        private string GetJsonPath()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(baseDir, "TestResources", "countries.json");
        }

        [TestInitialize]
        public void Init()
        {
            var field = typeof(CountryList).GetField("_cache", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, null);
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void All_ShouldDeserializeJsonFile()
        {
            var path = GetJsonPath();
            var countries = CountryList.All(path);
            Assert.AreEqual(2, countries.Count);
            Assert.AreEqual("US", countries[0].Code);
            Assert.AreEqual("United States", countries[0].Name);
        }

        [TestMethod]
        public void All_ShouldReturnCachedInstance_OnSubsequentCalls()
        {
            var path = GetJsonPath();
            var first = CountryList.All(path);
            var second = CountryList.All(path);
            Assert.AreSame(first, second);
        }
    }
}
