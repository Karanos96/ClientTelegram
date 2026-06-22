namespace ClientTelegram.Entity
{
    public class SessionEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Phonenumber { get; set; } = string.Empty;
        public List<MessageEntity> Messages { get; set; } = new List<MessageEntity>();
    }
}
