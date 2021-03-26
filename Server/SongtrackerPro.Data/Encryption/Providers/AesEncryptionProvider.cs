using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SongtrackerPro.Data.Encryption.Providers
{
    public class AesEncryptionProvider : IEncryptionProvider
    {
        public const int AesBlockSize = 128;
        public const int InitializationVectorSize = 16;

        public AesEncryptionProvider(string key)
        {
            _key = Convert.FromBase64String(key);
        }
        private readonly byte[] _key;

        public string Encrypt(string toEncrypt)
        {
            if (toEncrypt == null)
                return null;

            if (toEncrypt == string.Empty)
                return string.Empty;

            using var cryptoServiceProvider = CreateCryptographyProvider();
            cryptoServiceProvider.GenerateIV();
            var initializationVector = cryptoServiceProvider.IV;

            using var encryptor = cryptoServiceProvider.CreateEncryptor(_key, initializationVector);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            var unencrypted = Encoding.UTF8.GetBytes(toEncrypt);
            memoryStream.Write(initializationVector, 0, initializationVector.Length);
            cryptoStream.Write(unencrypted, 0, unencrypted.Length);
            cryptoStream.FlushFinalBlock();

            var encrypted = memoryStream.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string toDecrypt)
        {
            if (toDecrypt == null)
                return null;

            if (toDecrypt == string.Empty)
                return string.Empty;

            var input = Convert.FromBase64String(toDecrypt);
            using var memoryStream = new MemoryStream(input);
            var initializationVector = new byte[InitializationVectorSize];

            memoryStream.Read(initializationVector, 0, initializationVector.Length);
            using var cryptoServiceProvider = CreateCryptographyProvider();
            using var decryptor = cryptoServiceProvider.CreateDecryptor(_key, initializationVector);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            var decrypted = streamReader.ReadToEnd().Trim('\0');
            return decrypted;
        }

        public static string GenerateKey(int keySize = 256)
        {
            using var cryptoServiceProvider = new AesCryptoServiceProvider
            {
                KeySize = keySize,
                BlockSize = AesBlockSize
            };

            cryptoServiceProvider.GenerateKey();

            return Convert.ToBase64String(cryptoServiceProvider.Key);
        }

        private AesCryptoServiceProvider CreateCryptographyProvider()
        {
            return new AesCryptoServiceProvider
            {
                BlockSize = AesBlockSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = _key,
                KeySize = _key.Length * 8
            };
        }
    }
}
