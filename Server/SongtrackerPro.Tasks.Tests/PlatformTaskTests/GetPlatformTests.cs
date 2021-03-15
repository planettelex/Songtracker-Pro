using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Tasks.Tests.PlatformTaskTests
{
    [TestClass]
    public class GetPlatformTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var seedServices = new SeedServices(DbContext);
            seedServices.DoTask(null);
            var seedPlatforms = new SeedPlatforms(DbContext, new ListServices(DbContext), new AddPlatform(DbContext));
            seedPlatforms.DoTask(null);

            var listPlatforms = new ListPlatforms(DbContext);
            var allPlatforms = listPlatforms.DoTask(null);
            var randomIndex = new Random().Next(allPlatforms.Data.Count);
            var randomPlatform = allPlatforms.Data[randomIndex];
            Assert.IsNotNull(randomPlatform);

            var task = new GetPlatform(DbContext);
            var result = task.DoTask(randomPlatform.Id);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var platform = result.Data;
            Assert.AreEqual(randomPlatform.Name, platform.Name);
            Assert.AreEqual(randomPlatform.Website, platform.Website);
            Assert.IsNotNull(platform.Services);
            Assert.IsTrue(platform.Services.Any());
            foreach (var service in platform.Services)
            {
                Assert.IsTrue(randomPlatform.Services.Exists(s => s.Id == service.Id));
            }
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new GetPlatform(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
