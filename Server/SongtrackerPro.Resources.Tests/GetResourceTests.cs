using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SongtrackerPro.Resources.Tests
{
    [TestClass]
    public class GetResourceTests
    {
        [TestMethod]
        public void SeedDataTest()
        {
            foreach (var culture in AssemblyInfo.SupportedCultures)
                foreach (var key in AssemblyInfo.SeedDataKeys)
                    Assert.IsNotNull(GetResource.SeedData(culture, key));
        }

        [TestMethod]
        public void SystemMessagesTest()
        {
            foreach (var culture in AssemblyInfo.SupportedCultures)
                foreach (var key in AssemblyInfo.SystemMessageKeys)
                    Assert.IsNotNull(GetResource.SystemMessage(culture, key));
        }

        [TestMethod]
        public void EmailTemplatesTest()
        {
            foreach (var culture in AssemblyInfo.SupportedCultures)
                foreach (var filename in AssemblyInfo.EmailTemplates)
                    Assert.IsNotNull(GetResource.EmailTemplate(culture, filename));
        }
    }
}
