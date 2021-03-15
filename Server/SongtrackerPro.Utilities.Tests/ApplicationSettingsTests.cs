using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SongtrackerPro.Utilities.Tests
{
    [TestClass]
    public class ApplicationSettingsTests
    {
        [TestMethod]
        public void ConnectionStringTest()
        {
            Assert.IsNotNull(ApplicationSettings.Database.ConnectionString);
            Assert.IsTrue(ApplicationSettings.Database.ConnectionString.Length > 0);
        }
    }
}
