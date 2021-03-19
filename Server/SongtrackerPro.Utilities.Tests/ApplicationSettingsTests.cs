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

        [TestMethod]
        public void SmtpTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.Smtp);
            Assert.IsTrue(ApplicationSettings.Mail.Smtp.Length > 0);
        }

        [TestMethod]
        public void PortTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.Port);
            Assert.IsTrue(ApplicationSettings.Mail.Port > 0);
        }

        [TestMethod]
        public void EnableSslTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.EnableSsl);
        }

        [TestMethod]
        public void FromTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.From);
            Assert.IsTrue(ApplicationSettings.Mail.From.Length > 0);
        }

        [TestMethod]
        public void PasswordTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.Password);
            Assert.IsTrue(ApplicationSettings.Mail.Password.Length > 0);
        }
    }
}
