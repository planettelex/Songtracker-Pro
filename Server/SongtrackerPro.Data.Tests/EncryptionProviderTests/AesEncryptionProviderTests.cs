using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Encryption.Providers;

namespace SongtrackerPro.Data.Tests.EncryptionProviderTests
{
    [TestClass]
    public class AesEncryptionProviderTests
    {
        [TestMethod]
        public void CreateKeyTest()
        {
            var key = AesEncryptionProvider.GenerateKey();
            Assert.IsNotNull(key);
            Assert.IsTrue(key.Length > 0);
        }

        [TestMethod]
        public void EncryptionTests()
        {
            var key = AesEncryptionProvider.GenerateKey();
            Assert.IsNotNull(key);
            Assert.IsTrue(key.Length > 0);

            var aes = new AesEncryptionProvider(key);
            const string toEncrypt = "A man, a plan, a canal. Panama.";
            
            var encrypted = aes.Encrypt(toEncrypt);
            Assert.IsNotNull(encrypted);
            Assert.IsTrue(encrypted.Length > 0);
            Assert.AreNotEqual(toEncrypt, encrypted);

            var decrypted = aes.Decrypt(encrypted);
            Assert.IsNotNull(decrypted);
            Assert.IsTrue(decrypted.Length > 0);
            Assert.AreEqual(toEncrypt, decrypted);
        }
    }
}
