namespace ClientTelegram.Entity
{
    public class MessageEntity : BaseEntity
    {
        public int Id { get; set; }                   
        public int SessionId { get; set; }    
        public SessionEntity? Session { get; set; }
        public long ChatId { get; set; }
        public long MessageId { get; set; }           
        public string Type { get; set; } = "";         // text, photo, video...
        public bool IsOutgoing { get; set; }
        public DateTime Date { get; set; }

        public byte[] Ciphertext { get; set; } = [];
        public byte[] Nonce { get; set; } = [];
        public byte[] Tag { get; set; } = [];
        public int KeyId { get; set; }
    }
}
