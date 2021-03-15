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
    public class ListServicesTests
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Nothing, List<Service>> task = new ListServices(new ApplicationDbContext(ApplicationSettings.ConnectionString));
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
            ITask<Nothing, List<Service>> task = new ListServices(new ApplicationDbContext(string.Empty));
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
