using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.Tests.PlatformTaskTests
{
    [TestClass]
    public class UpdatePlatformTests
    {
        public readonly Platform Platform = new Platform
        {
            Id = 4,
            Name = "Update " + DateTime.Now.Ticks,
            Services = new List<Service>
            {
                new Service { Id = 2 },
                new Service { Id = 4 },
                new Service { Id = 5 }
            }
        };
        
        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Platform, Nothing> updateTask = new UpdatePlatform(new ApplicationDbContext(ApplicationSettings.ConnectionString));
            var toUpdate = Platform;
            var updateResult = updateTask.DoTask(toUpdate);
            
            Assert.IsTrue(updateResult.Success);
            Assert.IsNull(updateResult.Exception);
            Assert.IsNull(updateResult.Data);

            ITask<int, Platform> getTask = new GetPlatform(new ApplicationDbContext(ApplicationSettings.ConnectionString));
            var getResult = getTask.DoTask(toUpdate.Id);

            Assert.IsTrue(getResult.Success);
            Assert.IsNotNull(getResult.Data);

            var platform = getResult.Data;
            Assert.AreEqual(toUpdate.Name, platform.Name);
            Assert.AreEqual(toUpdate.Services.Count, platform.PlatformServices.Count);
            foreach (var service in toUpdate.Services)
            {
                var platformService = platform.PlatformServices.SingleOrDefault(ps => ps.ServiceId == service.Id);
                Assert.IsNotNull(platformService);
            }
        }

        [TestMethod]
        public void TaskFailTest()
        {
            ITask<Platform, Nothing> task = new UpdatePlatform(new ApplicationDbContext(string.Empty));
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
