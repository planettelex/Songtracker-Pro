using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.Tests.GeographicTaskTests
{
    [TestClass]
    public class ListCountriesTests
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Nothing, List<Country>> task = new ListCountries(new ApplicationDbContext(ApplicationSettings.ConnectionString));
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
            ITask<Nothing, List<Country>> task = new ListCountries(new ApplicationDbContext(string.Empty));
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
