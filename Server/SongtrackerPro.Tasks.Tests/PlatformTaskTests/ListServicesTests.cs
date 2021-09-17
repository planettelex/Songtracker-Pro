using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Tasks.Tests.PlatformTaskTests
{
    [TestClass]
    public class ListServicesTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new ListServices(DbContext);
            var result = task.DoTask(ServiceType.Unspecified);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());

            foreach (var service in result.Data)
            {
                Assert.IsTrue(service.Name.Length > 0);
            }
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
