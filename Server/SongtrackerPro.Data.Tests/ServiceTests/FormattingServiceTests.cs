using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Data.Tests.ServiceTests
{
    [TestClass]
    public class FormattingServiceTests
    {
        [TestMethod]
        public void FormatTaxIdTest()
        {
            IFormattingService formattingService = new FormattingService();
            var taxId = "123456789";
            var formattedTaxId = formattingService.FormatTaxId(taxId);
            Assert.AreEqual("12-3456789", formattedTaxId);
            taxId = "98-7654321";
            formattedTaxId = formattingService.FormatTaxId(taxId);
            Assert.AreEqual("98-7654321", formattedTaxId);
            taxId = "33";
            formattedTaxId = formattingService.FormatTaxId(taxId);
            Assert.AreEqual("33", formattedTaxId);
            taxId = "334";
            formattedTaxId = formattingService.FormatTaxId(taxId);
            Assert.AreEqual("33-4", formattedTaxId);
        }

        [TestMethod]
        public void FormatSocialSecurityNumberTest()
        {
            IFormattingService formattingService = new FormattingService();
            var socialSecurityNumber = "354789876";
            var formattedSocialSecurityNumber = formattingService.FormatSocialSecurityNumber(socialSecurityNumber);
            Assert.AreEqual("354-78-9876", formattedSocialSecurityNumber);
            socialSecurityNumber = "987-65-4321";
            formattedSocialSecurityNumber = formattingService.FormatSocialSecurityNumber(socialSecurityNumber);
            Assert.AreEqual("987-65-4321", formattedSocialSecurityNumber);
            socialSecurityNumber = "444";
            formattedSocialSecurityNumber = formattingService.FormatSocialSecurityNumber(socialSecurityNumber);
            Assert.AreEqual("444", formattedSocialSecurityNumber);
            socialSecurityNumber = "44455";
            formattedSocialSecurityNumber = formattingService.FormatSocialSecurityNumber(socialSecurityNumber);
            Assert.AreEqual("444-55", formattedSocialSecurityNumber);
            socialSecurityNumber = "444556";
            formattedSocialSecurityNumber = formattingService.FormatSocialSecurityNumber(socialSecurityNumber);
            Assert.AreEqual("444-55-6", formattedSocialSecurityNumber);
        }

        [TestMethod]
        public void FormatPhoneNumberTest()
        {
            IFormattingService formattingService = new FormattingService();
            var phoneNumber = "303";
            var formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("303", formattedPhoneNumber);
            phoneNumber = "3033";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("303-3", formattedPhoneNumber);
            phoneNumber = "3598207";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("359-8207", formattedPhoneNumber);
            phoneNumber = "359-8207";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("359-8207", formattedPhoneNumber);
            phoneNumber = "3033598207";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("(303) 359-8207", formattedPhoneNumber);
            phoneNumber = "(303)359-8207";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("(303) 359-8207", formattedPhoneNumber);
            phoneNumber = "30335988";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("(303) 359-88", formattedPhoneNumber);
            phoneNumber = "13033598207";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("+1 (303) 359-8207", formattedPhoneNumber);
            phoneNumber = "13033598207999";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("+1 (303) 359-8207 x999", formattedPhoneNumber);
            phoneNumber = "130335982079";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("+1 (303) 359-8207 x9", formattedPhoneNumber);
            phoneNumber = "1(303)359-8207 ext.999";
            formattedPhoneNumber = formattingService.FormatPhoneNumber(phoneNumber);
            Assert.AreEqual("+1 (303) 359-8207 x999", formattedPhoneNumber);
        }
    }
}
