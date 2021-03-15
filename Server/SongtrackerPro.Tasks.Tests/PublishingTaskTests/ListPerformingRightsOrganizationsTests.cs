using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class ListPerformingRightsOrganizationsTests
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Nothing, List<PerformingRightsOrganization>> task = new ListPerformingRightsOrganizations(new ApplicationDbContext(ApplicationSettings.ConnectionString));
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
            ITask<Nothing, List<PerformingRightsOrganization>> task = new ListPerformingRightsOrganizations(new ApplicationDbContext(string.Empty));
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
