using ClientTelegram.Context;
using ClientTelegram.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClientTelegram.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MessageEntity message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MessageEntity>> GetBySessionAsync(int sessionId)
        {
            return await _context.Messages
                .AsNoTracking()
                .Where(message => message.SessionId == sessionId)
                .OrderBy(message => message.Date)
                .ToListAsync();
        }
    }
}
