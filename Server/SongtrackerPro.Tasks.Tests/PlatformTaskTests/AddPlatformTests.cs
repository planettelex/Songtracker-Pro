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
        public readonly Platform NewPlatform = new Platform
        {
            Name = "Platform " + DateTime.Now.Ticks,
            Services = new List<Service>
            {
                new Service { Id = 1 },
                new Service { Id = 3 },
                new Service { Id = 5 }
            }
        };

        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Platform, int?> task = new AddPlatform(DbContext);
            var result = task.DoTask(NewPlatform);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var platformId = result.Data;
            Assert.IsTrue(platformId > 0);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            ITask<Platform, int?> task = new AddPlatform(EmptyDbContext);
            var result = task.DoTask(NewPlatform);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
