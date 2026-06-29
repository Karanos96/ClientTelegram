namespace ClientTelegram.OptionEntity
{
    public class CryptoOptions
    {
        public int ActiveKeyId { get; set; }
        public string MasterKeyBase64 { get; set; } = string.Empty;
    }
}
