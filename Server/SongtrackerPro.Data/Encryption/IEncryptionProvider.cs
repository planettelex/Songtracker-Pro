namespace SongtrackerPro.Data.Encryption
{
    public interface IEncryptionProvider
    {
        string Encrypt(string toEncrypt);

        string Decrypt(string toDecrypt);
    }
}
