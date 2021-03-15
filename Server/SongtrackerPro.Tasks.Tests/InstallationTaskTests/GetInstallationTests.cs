using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Tasks.Tests.InstallationTaskTests
{
    [TestClass]
    public class GetInstallationTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Nothing, bool> seedInstallation = new SeedInstallation(DbContext);
            seedInstallation.DoTask(null);
            
            ITask<Nothing, Installation> task = new GetInstallation(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.Uuid);
            Assert.IsNotNull(result.Data.Name);
            Assert.IsNotNull(result.Data.Version);
            Assert.IsNotNull(result.Data.OAuthId);
            Assert.IsNotNull(result.Data.DatabaseName);

            Assert.IsTrue(result.Data.Name.Length > 0);
            Assert.IsTrue(result.Data.Version.Length > 0);
            Assert.IsTrue(result.Data.OAuthId.Length > 0);
            Assert.IsTrue(result.Data.DatabaseName.Length > 0);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            ITask<Nothing, Installation> task = new GetInstallation(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
