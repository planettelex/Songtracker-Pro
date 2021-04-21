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
        public void ApiMinifyJsonTest()
        {
            Assert.AreEqual(ApplicationSettings.Api.MinifyJson, ApplicationSettings.Api.MinifyJson);
        }

        [TestMethod]
        public void ApiCultureTest()
        {
            Assert.IsNotNull(ApplicationSettings.Api.Culture);
            Assert.IsTrue(ApplicationSettings.Api.Culture.Length > 0);
        }

        [TestMethod]
        public void ApiCurrencyTest()
        {
            Assert.IsNotNull(ApplicationSettings.Api.Currency);
            Assert.IsTrue(ApplicationSettings.Api.Currency.Length > 0);
        }

        [TestMethod]
        public void WebDomainTest()
        {
            Assert.IsNotNull(ApplicationSettings.Web.Domain);
            Assert.IsTrue(ApplicationSettings.Web.Domain.Length > 0);
        }

        [TestMethod]
        public void WebOAuthIdTest()
        {
            Assert.IsNotNull(ApplicationSettings.Web.OAuthId);
            Assert.IsTrue(ApplicationSettings.Web.OAuthId.Length > 0);
        }

        [TestMethod]
        public void SmtpServerTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.Smtp);
            Assert.IsTrue(ApplicationSettings.Mail.Smtp.Length > 0);
        }

        [TestMethod]
        public void SmtpPortTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.Port);
            Assert.IsTrue(ApplicationSettings.Mail.Port > 0);
        }

        [TestMethod]
        public void SmtpEnableSslTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.EnableSsl);
        }

        [TestMethod]
        public void SmtpFromTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.From);
            Assert.IsTrue(ApplicationSettings.Mail.From.Length > 0);
        }

        [TestMethod]
        public void SmtpPasswordTest()
        {
            Assert.IsNotNull(ApplicationSettings.Mail.Password);
            Assert.IsTrue(ApplicationSettings.Mail.Password.Length > 0);
        }

        [TestMethod]
        public void DataEncryptionKeyTest()
        {
            Assert.IsNotNull(ApplicationSettings.Database.EncryptionKey);
            Assert.IsTrue(ApplicationSettings.Database.EncryptionKey.Length > 0);
        }
    }
}
