using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Utilities.Extensions;

namespace SongtrackerPro.Utilities.Tests.ExtensionTests
{
    [TestClass]
    public class StringExtensionTests
    {
        [TestMethod]
        public void SpaceUniformlyTest()
        {
            const string notUniformSpacing = " The  quick brown   fox jumps \tover  the lazy\r\n dog";
            const string uniformSpacing = "The quick brown fox jumps over the lazy dog";

            Assert.AreEqual(uniformSpacing, notUniformSpacing.SpaceUniformly());
        }
    }
}
