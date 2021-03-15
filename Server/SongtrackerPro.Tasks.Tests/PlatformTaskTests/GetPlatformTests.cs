using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Tasks.Tests.PlatformTaskTests
{
    [TestClass]
    public class GetPlatformTests : TestsBase
    {
        public const int TestPlatformId = 1;

        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<int, Platform> task = new GetPlatform(DbContext);
            var result = task.DoTask(TestPlatformId);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var platform = result.Data;
            Assert.IsTrue(platform.Name.Length > 0);
            Assert.IsNotNull(platform.Services);
            Assert.IsTrue(platform.Services.Any());
            foreach (var service in platform.Services)
            {
                Assert.IsTrue(service.Name.Length > 0);
            }
        }

        [TestMethod]
        public void TaskFailTest()
        {
            ITask<int, Platform> task = new GetPlatform(EmptyDbContext);
            var result = task.DoTask(TestPlatformId);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
