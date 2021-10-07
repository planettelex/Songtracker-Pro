using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class ListRecordingRolesTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new ListRecordingRoles(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Any());

            foreach (var role in result.Data)
                Assert.IsTrue(role.Name.Length > 0);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListRecordingRoles(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
