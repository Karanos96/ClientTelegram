using ClientTelegram.Entity;

namespace ClientTelegram.Repository
{
    public interface IMessageRepository
    {
        Task AddAsync(MessageEntity message);
        Task<List<MessageEntity>> GetBySessionAsync(int sessionId);
    }
}
