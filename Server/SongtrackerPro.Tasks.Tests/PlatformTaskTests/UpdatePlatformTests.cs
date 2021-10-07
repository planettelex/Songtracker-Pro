using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Tasks.Tests.PlatformTaskTests
{
    [TestClass]
    public class UpdatePlatformTests : TestsBase
    {
        public void UpdatePlatformModel(Platform platform)
        {
            var listServicesTask = new ListServices(DbContext);
            var allServices = listServicesTask.DoTask(ServiceType.Platform).Data;

            const int maxServiceCount = 5;
            var numberOfServices = new Random().Next(1, maxServiceCount);

            platform.Name = nameof(Platform) + " " + DateTime.Now.Ticks;
            platform.Website = "https://update.com";
            platform.Services = new List<Service>();
            for (var i = 0; i < numberOfServices; i++)
            {
                var randomIndex = new Random().Next(allServices.Count - 1);
                platform.Services.Add(allServices[randomIndex]);
            }
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var listPlatformsTask = new ListPlatforms(DbContext);
            var allPlatforms = listPlatformsTask.DoTask(null);
            var randomIndex = new Random().Next(allPlatforms.Data.Count - 1);
            var randomPlatform = allPlatforms.Data[randomIndex];
            Assert.IsNotNull(randomPlatform);

            var oldPlatform = new Platform
            {
                Id = randomPlatform.Id,
                Name = randomPlatform.Name,
                Website = randomPlatform.Website,
                Services = randomPlatform.Services
            };

            var task = new UpdatePlatform(DbContext);
            var toUpdate = randomPlatform;
            UpdatePlatformModel(toUpdate);
            var result = task.DoTask(toUpdate);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getPlatformTask = new GetPlatform(DbContext);
            var platform = getPlatformTask.DoTask(toUpdate.Id)?.Data;

            Assert.IsNotNull(platform);
            Assert.AreEqual(toUpdate.Name, platform.Name);
            Assert.AreEqual(toUpdate.Website, platform.Website);

            foreach (var service in toUpdate.Services)
            {
                var platformService = platform.PlatformServices.SingleOrDefault(ps => ps.ServiceId == service.Id);
                Assert.IsNotNull(platformService);
            }

            var revertPlatformResult = task.DoTask(oldPlatform);

            Assert.IsTrue(revertPlatformResult.Success);
            Assert.IsNull(revertPlatformResult.Exception);

            getPlatformTask = new GetPlatform(DbContext);
            platform = getPlatformTask.DoTask(oldPlatform.Id)?.Data;

            Assert.IsNotNull(platform);
            Assert.AreEqual(oldPlatform.Name, platform.Name);
            Assert.AreEqual(oldPlatform.Website, platform.Website);
            Assert.AreEqual(oldPlatform.Services.Count, platform.PlatformServices.Count);
            foreach (var service in oldPlatform.Services)
            {
                var platformService = platform.PlatformServices.SingleOrDefault(ps => ps.ServiceId == service.Id);
                Assert.IsNotNull(platformService);
            }
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdatePlatform(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
