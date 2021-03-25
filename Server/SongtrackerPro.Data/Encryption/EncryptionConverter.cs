using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SongtrackerPro.Data.Encryption
{
    internal sealed class EncryptionConverter : ValueConverter<string, string>
    {
        public EncryptionConverter(IEncryptionProvider encryptionProvider, ConverterMappingHints mappingHints = null) 
            : base(s => encryptionProvider.Encrypt(s), s => encryptionProvider.Decrypt(s), mappingHints)
        {
        }
    }
}
