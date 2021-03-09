using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SongtrackerPro.Utilities.Tests
{
    [TestClass]
    public class ApplicationSettingsTests
    {
        [TestMethod]
        public void TestApplicationSettings()
        {
            Assert.IsNotNull(ApplicationSettings.ConnectionString);
            Assert.IsTrue(ApplicationSettings.ConnectionString.Length > 0);
        }
    }
}
