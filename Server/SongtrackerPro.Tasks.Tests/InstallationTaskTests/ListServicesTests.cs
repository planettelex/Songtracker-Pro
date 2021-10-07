using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Tasks.Tests.InstallationTaskTests
{
    [TestClass]
    public class ListServicesTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new ListServices(DbContext);
            var platformServicesResult = task.DoTask(ServiceType.Platform);
            
            Assert.IsTrue(platformServicesResult.Success);
            Assert.IsNull(platformServicesResult.Exception);
            Assert.IsNotNull(platformServicesResult.Data);
            Assert.IsTrue(platformServicesResult.Data.Any());

            foreach (var service in platformServicesResult.Data)
            {
                Assert.IsTrue(service.Name.Length > 0);
            }

            var legalEntityServicesResult = task.DoTask(ServiceType.LegalEntity);
            
            Assert.IsTrue(legalEntityServicesResult.Success);
            Assert.IsNull(legalEntityServicesResult.Exception);
            Assert.IsNotNull(legalEntityServicesResult.Data);
            Assert.IsTrue(legalEntityServicesResult.Data.Any());

            foreach (var service in legalEntityServicesResult.Data)
            {
                Assert.IsTrue(service.Name.Length > 0);
            }

            var allServicesResult = task.DoTask(ServiceType.Unspecified);

            Assert.IsTrue(allServicesResult.Success);
            Assert.IsNull(allServicesResult.Exception);
            Assert.IsNotNull(allServicesResult.Data);
            Assert.IsTrue(allServicesResult.Data.Any());
            Assert.IsTrue(allServicesResult.Data.Count == platformServicesResult.Data.Count + legalEntityServicesResult.Data.Count);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListServices(EmptyDbContext);
            var result = task.DoTask(ServiceType.Unspecified);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
