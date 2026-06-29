namespace ClientTelegram.Security
{
    public interface IMessageCryptoService
    {
        EncryptedPayload Encrypt(string plaintext);
        string Decrypt(EncryptedPayload payload);
    }
}
