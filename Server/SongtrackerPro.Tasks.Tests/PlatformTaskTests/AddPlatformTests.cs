using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Tasks.Tests.PlatformTaskTests
{
    [TestClass]
    public class AddPlatformTests : TestsBase
    {
        public Platform CreateTestPlatform()
        {
            var listServicesTask = new ListServices(DbContext);
            var allServices = listServicesTask.DoTask(null).Data;

            const int maxServiceCount = 5;
            var numberOfServices = new Random().Next(1, maxServiceCount);

            var services = new List<Service>();
            for (var i = 0; i < numberOfServices; i++)
            {
                var randomIndex = new Random().Next(allServices.Count - 1);
                services.Add(allServices[randomIndex]);
            }

            return new Platform
            {
                Name = nameof(Platform) + " " + DateTime.Now.Ticks,
                Website = "https://platform.com",
                Services = services
            };
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddPlatform(DbContext);
            var testPlatform = CreateTestPlatform();
            var result = task.DoTask(testPlatform);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var platformId = result.Data;
            Assert.IsNotNull(platformId);
            Assert.IsTrue(platformId > 0);

            var getPlatformTask = new GetPlatform(DbContext);
            var platform = getPlatformTask.DoTask(platformId.Value)?.Data;

            Assert.IsNotNull(platform);
            Assert.AreEqual(testPlatform.Name, platform.Name);
            Assert.AreEqual(testPlatform.Website, platform.Website);
            Assert.AreEqual(testPlatform.Services.Count, platform.Services.Count);
            foreach (var service in testPlatform.Services)
            {
                Assert.IsTrue(platform.Services.Exists(s => s.Id == service.Id));
            }

            var removePlatformTask = new RemovePlatform(DbContext);
            var removeResult = removePlatformTask.DoTask(platform);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            ITask<Platform, int?> task = new AddPlatform(EmptyDbContext);
            var result = task.DoTask(new Platform());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
