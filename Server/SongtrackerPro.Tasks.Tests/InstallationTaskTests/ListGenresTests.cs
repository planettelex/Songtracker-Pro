using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Tasks.Tests.InstallationTaskTests
{
    [TestClass]
    public class ListGenresTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new ListGenres(DbContext);
            var allGenresResult = task.DoTask(null);

            Assert.IsTrue(allGenresResult.Success);
            Assert.IsNull(allGenresResult.Exception);
            Assert.IsNotNull(allGenresResult.Data);
            Assert.IsTrue(allGenresResult.Data.Any());
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListGenres(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
