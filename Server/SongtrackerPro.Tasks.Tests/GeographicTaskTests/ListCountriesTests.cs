using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.GeographicTasks;

namespace SongtrackerPro.Tasks.Tests.GeographicTaskTests
{
    [TestClass]
    public class ListCountriesTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var seedCountries = new SeedCountries(DbContext);
            seedCountries.DoTask(null);
            
            var task = new ListCountries(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());

            foreach (var country in result.Data)
            {
                Assert.IsTrue(country.Name.Length > 0);
                Assert.IsTrue(country.IsoCode.Length > 0);
            }
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListCountries(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
