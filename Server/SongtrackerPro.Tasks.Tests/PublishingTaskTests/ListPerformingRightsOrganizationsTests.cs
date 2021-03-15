using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class ListPerformingRightsOrganizationsTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Nothing, bool> seedPros = new SeedPerformingRightsOrganizations(DbContext, new SeedCountries(DbContext));
            seedPros.DoTask(null);

            ITask<Nothing, List<PerformingRightsOrganization>> task = new ListPerformingRightsOrganizations(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());

            foreach (var performingRightsOrganization in result.Data)
            {
                Assert.IsTrue(performingRightsOrganization.Name.Length > 0);
                Assert.IsNotNull(performingRightsOrganization.Country);
                Assert.IsTrue(performingRightsOrganization.Country.Name.Length > 0);
                Assert.IsTrue(performingRightsOrganization.Country.IsoCode.Length > 0);
            }
        }

        [TestMethod]
        public void TaskFailTest()
        {
            ITask<Nothing, List<PerformingRightsOrganization>> task = new ListPerformingRightsOrganizations(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
