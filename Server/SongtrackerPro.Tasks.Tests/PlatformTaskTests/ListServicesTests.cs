using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Tasks.Tests.PlatformTaskTests
{
    [TestClass]
    public class ListServicesTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var seedServices = new SeedServices(DbContext);
            seedServices.DoTask(null);

            var task = new ListServices(DbContext);
            var result = task.DoTask(null);
            
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
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
