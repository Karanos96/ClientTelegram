namespace ClientTelegram.Security
{
    public class EncryptedPayload
    {
        public byte[] Ciphertext { get; set; } = [];
        public byte[] Nonce { get; set; } = [];
        public byte[] Tag { get; set; } = [];
        public int KeyId { get; set; }
    }
}
