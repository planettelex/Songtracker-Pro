using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.Tests.InstallationTaskTests
{
    [TestClass]
    public class GetInstallationTaskTests
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            ITask<Installation> task = new GetInstallationTask(new ApplicationDbContext(ApplicationSettings.ConnectionString));
            var result = task.DoTask();
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.Uuid);
            Assert.IsNotNull(result.Data.Name);
            Assert.IsNotNull(result.Data.Version);
            Assert.IsNotNull(result.Data.OAuthId);

            Assert.IsTrue(result.Data.Name.Length > 0);
            Assert.IsTrue(result.Data.Version.Length > 0);
            Assert.IsTrue(result.Data.OAuthId.Length > 0);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            ITask<Installation> task = new GetInstallationTask(new ApplicationDbContext(string.Empty));
            var result = task.DoTask();
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
