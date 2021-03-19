using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Utilities.Services;

namespace SongtrackerPro.Utilities.Tests.ServiceTests
{
    [TestClass]
    public class HtmlServiceTests
    {
        [TestMethod]
        public void GetTitleTest()
        {
            IHtmlService htmlService = new HtmlService();
            const string htmlDocument = "<!DOCTYPE html><html><head><title>Title of HTML Document</title></head><body><h1>Heading</h1><p>Paragraph.</p></body></html>";
            Assert.AreEqual("Title of HTML Document", htmlService.GetTitle(htmlDocument));
        }
    }
}
