using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Tasks.Tests.PlatformTaskTests
{
    [TestClass]
    public class ListPlatformsTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Nothing, List<Platform>> task = new ListPlatforms(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());

            foreach (var platform in result.Data)
            {
                Assert.IsTrue(platform.Name.Length > 0);
                Assert.IsNotNull(platform.Services);
                if (platform.Services.Any())
                {
                    foreach (var service in platform.Services)
                    {
                        Assert.IsTrue(service.Name.Length > 0);
                    }
                }
            }
        }

        [TestMethod]
        public void TaskFailTest()
        {
            ITask<Nothing, List<Platform>> task = new ListPlatforms(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
