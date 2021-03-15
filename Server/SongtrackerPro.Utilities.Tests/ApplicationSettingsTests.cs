using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SongtrackerPro.Utilities.Tests
{
    [TestClass]
    public class ApplicationSettingsTests
    {
        [TestMethod]
        public void VersionTest()
        {
            Assert.IsNotNull(ApplicationSettings.Version);
            Assert.IsTrue(ApplicationSettings.Version.Length > 0);
        }
        
        [TestMethod]
        public void ConnectionStringTest()
        {
            Assert.IsNotNull(ApplicationSettings.Database.ConnectionString);
            Assert.IsTrue(ApplicationSettings.Database.ConnectionString.Length > 0);
        }

        [TestMethod]
        public void MinifyJsonTest()
        {
            Assert.AreEqual(ApplicationSettings.Api.MinifyJson, ApplicationSettings.Api.MinifyJson);
        }

        [TestMethod]
        public void OAuthIdTest()
        {
            Assert.IsNotNull(ApplicationSettings.Web.OAuthId);
            Assert.IsTrue(ApplicationSettings.Web.OAuthId.Length > 0);
        }
    }
}
