using ClientTelegram.Constant;
using ClientTelegram.OptionEntity;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ClientTelegram.Security
{
    public class MessageCryptoService : IMessageCryptoService
    {
        private const int TagSizeBytes = 16;
        private readonly CounterNonceGenerator _nonceGenerator;
        private readonly CryptoOptions _options;
        private readonly byte[] _key;

        public MessageCryptoService(
            IOptions<CryptoOptions> options,
            CounterNonceGenerator nonceGenerator)
        {
            _options = options.Value;
            _nonceGenerator = nonceGenerator;

            if(_options.ActiveKeyId <= 0)
            {
                throw new InvalidOperationException(ErrorMessage.ERROR_CRYPTO_ACTIVE_KEY_ID);
            }

            if (string.IsNullOrWhiteSpace(_options.MasterKeyBase64))
            {
                throw new InvalidOperationException(ErrorMessage.ERROR_MASTER_KEY);
            }

            _key = Convert.FromBase64String(_options.MasterKeyBase64);

            if(_key.Length != 32)
            {
                throw new InvalidOperationException(ErrorMessage.ERROR_AES_KEY);
            }
        }
        public string Decrypt(EncryptedPayload payload)
        {
            if (payload.KeyId != _options.ActiveKeyId)
                throw new InvalidOperationException(ErrorMessage.ERROR_PAYLOAD_DECRYPT + payload.KeyId);

            byte[] plaintextBytes = new byte[payload.Ciphertext.Length];

            using var aes = new AesGcm(_key, TagSizeBytes);
            aes.Decrypt(payload.Nonce, payload.Ciphertext, payload.Tag, plaintextBytes);

            return Encoding.UTF8.GetString(plaintextBytes);
        }

        public EncryptedPayload Encrypt(string plaintext)
        {
            byte[] nonce = _nonceGenerator.Next();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] ciphertext = new byte[plainTextBytes.Length];
            byte[] tag = new byte[TagSizeBytes];

            using var aes = new AesGcm(_key, TagSizeBytes);
            aes.Encrypt(nonce, plainTextBytes, ciphertext, tag);

            return new EncryptedPayload
            {
                Ciphertext = ciphertext,
                Nonce = nonce,
                Tag = tag,
                KeyId = _options.ActiveKeyId
            };
        }
    }
}
